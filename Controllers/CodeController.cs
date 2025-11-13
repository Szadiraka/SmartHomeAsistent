using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartHomeAsistent.CustomExceptions;
using SmartHomeAsistent.Entities;
using SmartHomeAsistent.services.classes;
using SmartHomeAsistent.services.interfaces;


namespace SmartHomeAsistent.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class CodeController : ControllerBase
    {
        private readonly ICodeService _codeService;
        private readonly IMessageService _messageService;
        private readonly IUserService _userService;

        public CodeController(ICodeService service, IMessageService messageService, IUserService userService)
        {
            _codeService = service;
            _messageService = messageService;
            _userService = userService;
        }

        [Authorize]
        [HttpPost("generateCode")]
        public async Task< IActionResult> GenerateCode([FromBody] int userId)
        {
            if (userId<=0)
                throw new ValidationException("Невалидный Id пользователя");

            User user =await _userService.GetUserById(userId);

            int code = GenerateRandomCode();
            await _codeService.CreateCode(userId, code, 5);

            //создаем сообщение в очереди
            BackgroundJob.Enqueue(() => _messageService.SendMessage(user.Email, null, null, code));

            return Ok(new
            {
                success = true,
                data = "Электронная почта подтверждена"
            });
        }



        [HttpPost("confirmEmail/{userId:int}")]
        [Authorize]
        public async Task<IActionResult> ConfirmEmail([FromRoute] int userId, [FromBody] int code)
        {         
            var result = await _codeService.ConfirmCodeAsync(userId, code);

            result = await _userService.ConfirmUserEmailStatusAsync(userId);

            //добавить подтвержение у пользователя
            if (result)
                return Ok(new
                {
                    success = true,
                    data = "Электронная почта подтверждена"
                });
            throw new ValidationException("Код не правильный либо истекло время подтверждения");
        }



        private int GenerateRandomCode()
        {
            return Random.Shared.Next(100000, 1000000);

        }
    }
}
