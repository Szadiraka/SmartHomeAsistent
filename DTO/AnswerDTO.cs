using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace SmartHomeAsistent.DTO
{
    public class AnswerDTO
    {
        [Required]
        public string Token { get; set; } = string.Empty;

        public bool   EmailConfirmed { get; set; }
    }
}
