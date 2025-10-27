using System.ComponentModel.DataAnnotations;

namespace SmartHomeAsistent.DTO
{
    public class DeviceDTO
    {
        [Required]
        [MinLength(3)]
        [MaxLength(50)]
        public string Name { get; set; } =string.Empty;


        [Required]
        [Range(1, int.MaxValue)]
        public int AccountId { get; set; }

        [Required]
        public string DeviceUniqueId { get; set; } = string.Empty;

        
        public decimal? SwitchingPower { get; set; }
    }
}
