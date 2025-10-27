using System.ComponentModel.DataAnnotations;

namespace SmartHomeAsistent.DTO
{
    public class RequestElementDTO
    {
        [Required]
        public string Id { get; set; } = string.Empty;
    }
}
