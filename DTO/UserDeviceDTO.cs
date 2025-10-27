using SmartHomeAsistent.Enumes;
using System.ComponentModel.DataAnnotations;

namespace SmartHomeAsistent.DTO
{
    public class UserDeviceDTO
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int UserId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int DeviceId { get; set; }

        [Required]
        public PermisionLevel Level { get; set; } 
    }
}
