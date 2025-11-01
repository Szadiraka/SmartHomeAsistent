using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;
using System.ComponentModel.DataAnnotations;

namespace SmartHomeAsistent.DTO
{
    public class AnswerDTO
    {
        [Required]
        public string Token { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        public bool   EmailConfirmed { get; set; }

        public required int UserId { get; set; }
    }
}
