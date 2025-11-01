using SmartHomeAsistent.services.interfaces;
using System.Net.Mail;
using System.Net;
using SmartHomeAsistent.Infrastructure;

namespace SmartHomeAsistent.services.classes
{
    public class EmailService : IMessageService
    {
        private readonly IConfiguration _configuration;
        private readonly EmailSettings _settings;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
            _settings = _configuration.GetSection("EmailService")
            .Get<EmailSettings>() ?? throw new Exception("Свойства не найдены");

        }




        public async Task SendMessage(string toEmail, string? subject, string? message, int? code)
        {       

            using var client = new SmtpClient(_settings.SmtpServer, _settings.Port)
            {
                Credentials = new NetworkCredential(_settings.FromEmail, _settings.Password),
                EnableSsl = true
            };
             code??= GenerateRandomCode();
             subject??= "Подтверждение электронной почты";
             message??= await GetMessageAsync(code.Value);


            var mailMessage = new MailMessage(_settings.FromEmail, toEmail, subject, message);
            mailMessage.IsBodyHtml = true;


            await client.SendMailAsync(mailMessage);
        }


        //-----------------------------------------------------------

    

         private async Task<string> GetMessageAsync (int code)
        {
            string templatePath = Path.Combine(AppContext.BaseDirectory, "Templates", "EmailConfirmation.html");   
            string body = await System.IO.File.ReadAllTextAsync(templatePath);
            body = body.Replace("{{CODE}}", code.ToString());
            return body;
           
        }



        private int GenerateRandomCode()
        {
            return Random.Shared.Next(100000, 1000000);
        }
 

     
    }
   
}
