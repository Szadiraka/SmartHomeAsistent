using SmartHomeAsistent.DTO;

namespace SmartHomeAsistent.services.interfaces
{
    public interface IEventHubService
    {
        public Task SendMessageAsync(RelayStatusDTO status);
    }
}
