using Microsoft.AspNetCore.Mvc;
using SmartHomeAsistent.DTO;
using SmartHomeAsistent.services.interfaces;

namespace SmartHomeAsistent.Controllers
{

    [ApiController]
    [Route("api/[controller]")]

    public class EventsController : ControllerBase
    {
        private readonly IEventHubService _eventHubService;

        public EventsController(IEventHubService eventHubService)
        {
            _eventHubService = eventHubService;
        }

        [HttpPost("messages")]
        public async Task <IActionResult> GetEvents([FromBody] RelayStatusDTO status)
        {
            await _eventHubService.SendMessageAsync(status);

            //Console.WriteLine("мыы получили сообщение и должны были отправить его в UI");
            return Ok(new { message = "Сообщение получено" });
        }
    }
}
