using System.ComponentModel.DataAnnotations;

namespace SmartHomeAsistent.Entities
{
    public class Code
    {
        [Key]
        public int Id { get; set; }

        [Range(1000,9999)]
        public int Data { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public DateTime ExpiresAt { get; set; }

        public int UserId { get; set; } 
        public User? User { get; set; }

    }
}
