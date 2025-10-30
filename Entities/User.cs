
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Security.Principal;

namespace SmartHomeAsistent.Entities
{

    [Index(nameof(Email), IsUnique = true)]
    public class User
    {
        public int Id { get; set; }


        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]        
        public string Email { get; set; } = string.Empty;

        //добавим новое поле- для отслеживания подтвержденной почты
        public bool EmailConfirmed { get; set; } = false;

        [Required]      
        
        public string PasswordHash { get; set; } = string.Empty;

        public bool IsBlocked { get; set; } = false;

        // если пользователь удален (скрыт)

        public bool Hidden { get; set; } = false;


        // связи
        public int RoleId { get; set; }
        public Role Role { get; set; } = null!;

        public ICollection <Account> Accounts { get; set; } = [];
        public ICollection <UserDevice> UserDevices { get; set; } = [];
        public ICollection <RelayScenario>  RelayScenarios { get; set; } = [];

        public ICollection <Code> Codes { get; set; } = [];

    }

}
