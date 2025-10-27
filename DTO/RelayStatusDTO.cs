using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace SmartHomeAsistent.DTO
{
    public class RelayStatusDTO
    {
        [Required]
        public string Id { get; set; } = string.Empty;
        [Required]
        public string Status { get; set; } = string.Empty;


        override public string ToString()
        {
            return $"Id: {Id}, Status: {Status}";
        }
    }
}
