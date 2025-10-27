using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Constraints;
using SmartHomeAsistent.CustomExceptions;
using SmartHomeAsistent.DTO;
using SmartHomeAsistent.Entities;
using SmartHomeAsistent.services.interfaces;

namespace SmartHomeAsistent.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RelayScenarioController : ControllerBase
    {
        private readonly IRelayScenarioService _service;

        public RelayScenarioController(IRelayScenarioService service)
        {
            _service = service;
        }


        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetScenarioByIdAsync(int id)
        {
          
            var result  = await _service.GetRelayScenarioById(id);
            return Ok(new
            {
                success = true,
                data = result
            });


        }

        [HttpGet("byUser/{userId:int}")]
        public async Task<IActionResult> GetAllRelayScenariosByUserIdAsync(int userId)
        {
           
            var result = await _service.GetAllRelayScenariosByUserId(userId);
            return Ok(new
            {
                success = true,
                data = result
            });

        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateRelayScenarioAsync(int id, [FromBody] RelayScenarioDTO relayScenario)
        {
            if(!ModelState.IsValid)
                throw new ValidationException("Некорректные данные");
           
            var result = await _service.UpdateRelayScenario(id, relayScenario);
            return Ok(new
            {
                success = true,
                data = result
            });

        }

        [HttpDelete("{relayScenarioId:int}/{userId:int}")]
        public async Task<IActionResult> DeleteRelayScenarioAsync(int relayScenarioId, int userId)
        {
           
            var result = await _service.DeleteRelayScenario(relayScenarioId, userId);
            return Ok(new
            {
                success = true,
                data = result
            });

        }


        [HttpPost]
        public async Task<IActionResult> AddRelayScenarion([FromBody] RelayScenarioDTO relayScenario)
        {
           if(!ModelState.IsValid)
                throw new ValidationException("Некорректные данные");
            var result = await _service.AddRelayScenarion(relayScenario);
            return Ok(new
            {
                success = true,
                data = result
            });

        }       

    }
}
