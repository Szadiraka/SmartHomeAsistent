using System.ComponentModel.DataAnnotations;

namespace SmartHomeAsistent.Entities
{
    public class Account
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string AccessKey { get; set; } = string.Empty;

        [Required]
        public string SecretKey { get; set; } = string.Empty; 

        public int OwnerId { get; set; }
        public User Owner { get; set; } = null!;

        [Required]
        [MinLength(4)]
        [MaxLength(30)]
        public string Name { get; set; } = string.Empty;

        public ICollection <Device> Devices { get; set; } =[];
        public ICollection <User> SharedUsers { get; set; } = [];
       



    }
}
