namespace SmartHomeAsistent.DTO
{
    public class EmailMessageDTO
    {
        public required int UserId { get; set; }

        public required string To { get; set; }
        public string? Subject { get; set; }
        public string? Body { get; set; }

    }

    
}
