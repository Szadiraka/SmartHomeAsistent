using Microsoft.AspNetCore.SignalR;
using SmartHomeAsistent.DTO;
using SmartHomeAsistent.services.interfaces;
using SmartHomeAsistent.signalR;
using System.ComponentModel.Design;

namespace SmartHomeAsistent.services.classes
{


    public class EventHubService: IEventHubService
    {
        private readonly IHubContext<EventHub> _hubContext;

        public EventHubService(IHubContext<EventHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task SendMessageAsync(RelayStatusDTO status)
        {
            var payload = new
            {
                Id = status.Id,
                Status = status.Status,
                Time = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")
            };
            Console.WriteLine($"Отправляем сообщение: для UI");
            await _hubContext.Clients.All.SendAsync("DeviceEvent", payload);

            //тут нужно будет переработать и отправлять только тем, кто подписан на события
            Console.WriteLine($"Отправили сообщение: для UI");
        }

    }
}
