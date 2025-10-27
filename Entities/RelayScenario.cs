using System.ComponentModel.DataAnnotations;

namespace SmartHomeAsistent.Entities
{
    public class RelayScenario
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        public int UserId { get; set; }
        public User? User { get; set; }

        public ICollection <RelayCommand> Commands { get; set; } = [];
    }
}
