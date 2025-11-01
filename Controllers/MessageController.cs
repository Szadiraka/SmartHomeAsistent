using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartHomeAsistent.DTO;
using SmartHomeAsistent.services.interfaces;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace SmartHomeAsistent.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _messageService;
        private readonly ICodeService _codeService;

        public MessageController(IMessageService messageService, ICodeService codeService)
        {
            _messageService = messageService;
            _codeService = codeService;
        }




        [HttpPost("message")]
        public async Task<IActionResult> SendMessage([FromBody] EmailMessageDTO emailMessage)
        {
            int code = GenerateRandomCode();
            if (emailMessage.UserId > 0)
            {            

                // coхранить в  бд код с датой
                await _codeService.CreateCode(emailMessage.UserId, code, 5);
            }
           

            //вызываем сервис отправки собщений ч/з hangFire
             BackgroundJob.Enqueue(() =>  _messageService.SendMessage(emailMessage.To, emailMessage.Subject, emailMessage.Body, code));

            return Ok(new
            {
                success = true,
                message = "Письмо с кодом подтверждения отправлено на почту."
            });

        }




        [HttpPost("getCode")]
        [Authorize]
        public async Task<IActionResult> GenerateAndSendCodeToEmail()
        {
            Claim emailClaim = HttpContext.User.FindFirst(ClaimTypes.Email) ??
                throw new ValidationException("Пользователь не найден или email не указан");
            Claim userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier) ??
                throw new ValidationException("Пользователь не найден или id не указан");

            string email = emailClaim.Value;
            if (!int.TryParse(userIdClaim.Value, out int userId))
                throw new ValidationException("Некорректный идентификатор пользователя");



            int code = GenerateRandomCode();
            await _codeService.CreateCode(userId, code, 5);

            //вызываем сервис отправки собщений ч/з hangFire
            BackgroundJob.Enqueue(() => _messageService.SendMessage(email, null, null, code));



            return Ok(new
            {
                success = true,
                message = "Письмо с кодом подтверждения отправлено на почту."
            });

        }


        //----------------------------------------------------------
        private int GenerateRandomCode()
        {
            return Random.Shared.Next(100000, 1000000);

        }





    }
}
