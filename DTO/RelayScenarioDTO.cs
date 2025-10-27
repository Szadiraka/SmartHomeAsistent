using System.ComponentModel.DataAnnotations;

namespace SmartHomeAsistent.DTO
{
    public class RelayScenarioDTO
    {
        [Required]
        [MinLength(4)]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Range(1, int.MaxValue)]
        public int UserId { get; set; }
    }
}
