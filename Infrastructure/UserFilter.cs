namespace SmartHomeAsistent.Infrastructure
{
    public class UserFilter
    {

        public string? Name { get; set; }
        public string? Email { get; set; }
        public bool? IsBlocked { get; set; } 
        public bool? Hidden { get; set; }
        public string? RoleName { get; set; }
    }
}
