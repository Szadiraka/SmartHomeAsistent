using System.ComponentModel.DataAnnotations;

namespace SmartHomeAsistent.Entities
{
    public class Device
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string DeviceUniqueId { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        public int AccountId { get; set; }
        public Account? Account { get; set; }


        public decimal SwitchingPower { get; set; }

        public ICollection <DeviceLog> DeviceLogs {get;set;} = [];
        public ICollection <UserDevice> UserDevices { get; set; } = [];

    }
}
