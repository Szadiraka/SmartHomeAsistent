using System.ComponentModel.DataAnnotations;

namespace SmartHomeAsistent.Entities
{
    public class DeviceLog
    {
        [Key]
        public int Id { get; set; }

        
        public int DeviceId { get; set; }
        public Device? Device { get; set; }

        public bool IsOn { get; set; }
        public DateTime TimeStamp { get; set; } = DateTime.UtcNow;
    }
}
