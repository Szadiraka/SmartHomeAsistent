using System.ComponentModel.DataAnnotations;

namespace SmartHomeAsistent.DTO
{
    public class AccountDTO
    {
        [Required]
        public string AccessKey { get; set; } = string.Empty;

        [Required]
        public string SecretKey { get; set; } = string.Empty;

        [Range(1, int.MaxValue)]
        public int OwnerId { get; set; }

        [Required]
        [MinLength(4)]
        [MaxLength(30)]
        public string Name { get; set; } = string.Empty;
    }
}
