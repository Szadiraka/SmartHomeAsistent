using Microsoft.AspNetCore.Mvc;
using SmartHomeAsistent.DTO;
using SmartHomeAsistent.services.classes;
using SmartHomeAsistent.services.interfaces;
using System.Runtime.CompilerServices;
using SmartHomeAsistent.CustomExceptions;




namespace SmartHomeAsistent.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class RelayController : ControllerBase
    {

        private readonly IRelayService _relayService;

        public RelayController(IRelayService relayService)
        {
            _relayService = relayService;
        }
     


        [HttpPost("status")]
        public async Task<IActionResult> GetStatus([FromBody] RequestElementDTO element)
        {
            var result  = await _relayService.GetStatusAsync(element.Id);
            if(!result.Success)
                throw new BadRequestException(result.Message);
            return Ok(new
            {
                success = result.Success,
                data = result.Message
            });
           

        }

        [HttpPost("action")]
        public async Task<IActionResult> SwitchRelay([FromBody] RelayComandDTO comand)
        {
            var result = await _relayService.SwitchRelayAsync(comand.Id, comand.Comand);
            if (!result.Success)
                throw new BadRequestException(result.Message);
            return Ok(new
            {
                success = result.Success,
                data = result.Message
            });
           
        }

  


    }
}
