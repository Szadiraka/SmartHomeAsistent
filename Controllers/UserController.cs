using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using SmartHomeAsistent.CustomExceptions;
using SmartHomeAsistent.DTO;
using SmartHomeAsistent.Entities;
using SmartHomeAsistent.Infrastructure;
using SmartHomeAsistent.services.interfaces;


namespace SmartHomeAsistent.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;


        public UserController(IUserService userService)
        {
            _service = userService;
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
                data = new { user.Id, user.Email, roleName = user.Role.Name } 
            });       
  
           
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
           if(!ModelState.IsValid)
                throw new ValidationException("Ќекорректные данные");
            string token =await _service.Login(loginDto.Email, loginDto.Password);
            return Ok(new
            {
                success = true,
                data = token
            });
           
            
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



    }
}
