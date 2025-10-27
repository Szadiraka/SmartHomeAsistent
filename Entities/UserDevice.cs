using SmartHomeAsistent.Enumes;

namespace SmartHomeAsistent.Entities
{
    public class UserDevice
    {
        
        public int UserId { get; set; } 

        public int DeviceId { get; set; }

        public PermisionLevel Level { get; set; } = PermisionLevel.None;

        public DateTime ProvidedAt { get; set; } = DateTime.UtcNow;


    }
}
