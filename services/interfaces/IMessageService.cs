namespace SmartHomeAsistent.services.interfaces
{
    public interface IMessageService
    {

        Task SendMessage(string to, string? subject, string? message, int? code);
    }
}
