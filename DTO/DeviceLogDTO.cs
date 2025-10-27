using System.ComponentModel.DataAnnotations;

namespace SmartHomeAsistent.DTO
{
    public class DeviceLogDTO
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int DeviceId { get; set; }

        [Required]
        public bool IsOn { get; set; }
        
    }
}
