using SmartHomeAsistent.Entities;
using System.ComponentModel.DataAnnotations;

namespace SmartHomeAsistent.DTO
{
    public class UserDTO
    {     
     
            [Required]
            [MaxLength(50)]
            public string Name { get; set; } = string.Empty;

            [Required]
            [EmailAddress]
            public string Email { get; set; } = string.Empty;

            [Required]
            [MaxLength(20)]
            [MinLength(4)]
            public string Password { get; set; } = string.Empty;
          
            public string?  RoleName { get; set; }

          

        
    }
}
