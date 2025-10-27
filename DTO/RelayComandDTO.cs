using System.ComponentModel.DataAnnotations;

namespace SmartHomeAsistent.DTO
{
    public class RelayComandDTO
    {
        [Required]
        public string Id { get; set; } = string.Empty;
        [Required]
        public string Comand { get; set; } = string.Empty;
    }
}
