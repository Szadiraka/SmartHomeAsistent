using System.ComponentModel.DataAnnotations;

namespace SmartHomeAsistent.DTO
{
    public class SecondaryUserDTO
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int OwnerId { get; set; }


        [Required]
        [Range(1, int.MaxValue)]
        public int SecondaryUserId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int AccountId { get; set; }
    }
}
