using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using SmartHomeAsistent.CustomExceptions;
using SmartHomeAsistent.DTO;
using SmartHomeAsistent.Entities;
using SmartHomeAsistent.Infrastructure;
using SmartHomeAsistent.services.interfaces;
using System.Security.Claims;
using System.IO;
using Microsoft.Extensions.Primitives;
using Hangfire;


namespace SmartHomeAsistent.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;
        private readonly ICodeService _codeService;
        private readonly IMessageService _messageService;
      

      
        public UserController(IUserService userService, ICodeService codeService, IMessageService messageService)
        {
            _service = userService;
            _codeService = codeService;  
            _messageService = messageService;
           
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserDTO userDto)
        {
            if (!ModelState.IsValid)
                throw new ValidationException("Ќекорректные данные пользовател€");          
                User user = await _service.RegisterAsync(userDto);
            return Ok(new
            {
                success = true,
                data = "¬ы успешно зарегестрированы"
            });       
  
           
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
            if (!ModelState.IsValid)
                throw new ValidationException("Ќекорректные данные");
            AnswerDTO answer = await _service.Login(loginDto.Email, loginDto.Password);


            //----------------------
            if (!answer.EmailConfirmed)
            {
                // создаем код и сохран€ем в бд
                int code = GenerateRandomCode();
                await _codeService.CreateCode(answer.UserId,code,5);

                //создаем сообщение в очереди
                BackgroundJob.Enqueue(() => _messageService.SendMessage(answer.Email, null, null, code));
               
            }

            return Ok(new
            {
                success = answer.EmailConfirmed,  //передаем в UI - подтверждена ли почта
                data = answer.Token,

            });

        }
        

        [HttpPost("confirmEmail")]
        [Authorize]
        public  async Task<IActionResult> ConfirmEmail([FromBody] int code)
        {
            Claim userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier) ??
                  throw new ValidationException("ѕользователь не найден или токен недействителен");

            if (!int.TryParse(userIdClaim.Value, out int userId))
                throw new ValidationException("Ќекорректный индитификатор пользовател€");

            var result = await _codeService.ConfirmCodeAsync(userId, code);
            if (result)
                return Ok(new
                {
                    success = true,
                    data = "Ёлектронна€ почта подтверждена"
                });
            throw new ValidationException(" од не правильный либо истекло врем€ подтверждени€");
        }        


        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> GetAllAsync([FromBody] UserFilter? filter)
        {
           
            var result = await _service.GetAllUsersAsync(filter);
            return Ok(new
            {
                success = true,
                data = result 
            });

        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task< IActionResult> GetUserById(int id)
        {
           if(id<=0)
                throw new ValidationException("не валидные данные");
            var result =await _service.GetUserById(id);
            return Ok(new
            {
                success = true,
                data = result
            });


        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserDTO userDto)
        {
            if (!ModelState.IsValid)
                throw new ValidationException("не валидные данные");
           
            var result = await _service.UpdateUserAsync(id, userDto);
            return Ok(new
            {
                success = true,
                data = result
            });

        }

        [Authorize(Roles = "admin")]
        [HttpPut("block/{id}")]
        public async Task<IActionResult> BlockUser(int id)
        {
            if (id <= 0)
                throw new ValidationException("не валидные данные");
            var result = await _service.BlockUserAsync(id);
            return Ok(new
            {
                success = true,
                data = result
            });

        }


        [Authorize(Roles = "admin")]
        [HttpPut("unblock/{id}")]
        public async Task<IActionResult> UnBlockUser(int id)
        {
            if (id <= 0)
                throw new ValidationException("не валидные данные");
            var result = await _service.UnblockUserAsync(id);
            return Ok(new
            {
                success = true,
                data = result
            });


        }


        [Authorize(Roles = "admin")]
        [HttpPut("delete/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            if (id <= 0)
                throw new ValidationException("не валидные данные");

            var result = await _service.DeleteUserAsync(id);
            return Ok(new
            {
                success = true,
                data = result
            });

        }

        [Authorize(Roles = "admin")]
        [HttpPut("recovery/{id}")]
        public async Task<IActionResult> RecoveryUser(int id)
        {
            if (id <= 0)
                throw new ValidationException("не валидные данные");
            var result = await _service.RecoveryUserAsync(id);
            return Ok(new
            {
                success = true,
                data = result
            });

        }


        //--------------------------------------------------------------------------------//



        private int GenerateRandomCode()
        {
            return Random.Shared.Next(100000, 1000000);

        }



    }
}
