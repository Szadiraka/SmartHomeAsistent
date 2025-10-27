using SmartHomeAsistent.Enumes;
using System.ComponentModel.DataAnnotations;

namespace SmartHomeAsistent.Entities
{
    public class RelayCommand
    {
        [Key]
        public int Id { get; set; }


        public int RelayScenarioId { get; set; }
        public RelayScenario? Scenario { get; set; }


        public int DeviceId { get; set; }
        public Device? Device { get; set; }

        [Required]
        public RelayCommmandType CommandType { get; set; }

        [Required]
        public SheduleType ScheduleType { get; set; }

        public DateTime? StartTime { get; set; }

        public TimeSpan? Delay { get; set; }

        public RepeatSettings RepeatSettings { get; set; } = new RepeatSettings();
    }
}
