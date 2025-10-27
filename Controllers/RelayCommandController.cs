using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartHomeAsistent.CustomExceptions;
using SmartHomeAsistent.DTO;
using SmartHomeAsistent.Entities;
using SmartHomeAsistent.services.interfaces;
using System.Runtime.CompilerServices;

namespace SmartHomeAsistent.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RelayCommandController : ControllerBase
    {

        private readonly IRelayCommandService _service;

        public  RelayCommandController(IRelayCommandService service)
        {
            _service = service;
        }


        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetRelayCpommandById( int id)
        {
            var result = await _service.GetRelayCommandById(id);
            return Ok(new
            {
                success = true,
                data = result
            });

        }

        [HttpPost]
        public async Task<IActionResult> AddRelayCommandAsync([FromBody] RelayCommandDTO relayCommand)
        {
           
            if (!ModelState.IsValid)
                throw new ValidationException("Некорректные данные");
            var result = await _service.AddRelayCommand(relayCommand);
            return Ok(new
            {
                success = true,
                data = result
            });

        }


        [HttpGet("relayScenario/{relayscenarioId:int}")]
        public async Task<IActionResult> GetRelayCommandsByRelayScenarioIdAsync(int relayscenarioId)
        {
           
            var result = await _service.GetRelayCommandsByRelayScenarioId(relayscenarioId);
            return Ok(new
            {
                success = true,
                data = result
            });


        }


        [HttpDelete("{relayCommandId:int}/{userId:int}")]
        public async Task<IActionResult> DeleteRelayCommandAsync(int relayCommandId, int userId)
        {
           
            var result = await  _service.DeleteRelayCommand(relayCommandId, userId);
            return Ok(new
            {
                success = true,
                data = result
            });


        }

        [HttpPut("{relayCommandId:int}/{userId:int}")]
        public async Task<IActionResult> UpdateRelayCommandAsync(int relayCommandId, int userId, [FromBody] RelayCommandDTO relayCommand)
        {
          
            if(!ModelState.IsValid)
                throw new ValidationException("Некорректные данные");
            var result =await _service.UpdateRelayCommand(relayCommandId, userId, relayCommand);
            return Ok(new
            {
                success = true,
                data = result
            });

        }

    }
}
