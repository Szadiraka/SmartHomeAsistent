using SmartHomeAsistent.Entities;
using SmartHomeAsistent.Enumes;
using System.ComponentModel.DataAnnotations;

namespace SmartHomeAsistent.DTO
{
    public class RelayCommandDTO
    {
        [Required]
        [MinLength(2)]
        [MaxLength(100)]
        public string CommandName { get; set; } = string.Empty;

        [Required]
        [Range(1, int.MaxValue)]
        public int RelayScenarioId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int DeviceId { get; set; }

        [Required]
        public RelayCommandType CommandType { get; set; }

        [Required]
        public SheduleType SheduleType { get; set; }

        public DateTime? StartTime { get; set; }

        public TimeSpan? Delay { get; set; }

        [Required]
        public RepeatSettings RepeatSettings { get; set; } = null!;
    }
}
