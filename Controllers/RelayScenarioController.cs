using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Constraints;
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
            try
            {
                RelayScenario result  = await _service.GetRelayScenarioById(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            };
        }

        [HttpGet("byUser/{userId:int}")]
        public async Task<IActionResult> GetAllRelayScenariosByUserIdAsync(int userId)
        {
            try
            {
                var result = await _service.GetAllRelayScenariosByUserId(userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            };
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateRelayScenarioAsync(int id, [FromBody] RelayScenarioDTO relayScenario)
        {
            try
            {
                var result = await _service.UpdateRelayScenario(id, relayScenario);
                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            };
        }

        [HttpDelete("{relayScenarioId:int}/{userId:int}")]
        public async Task<IActionResult> DeleteRelayScenarioAsync(int relayScenarioId, int userId)
        {
            try
            {
                var result = await _service.DeleteRelayScenario(relayScenarioId, userId);
                return Ok(result);
            }catch(Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            };
        }


        [HttpPost]
        public async Task<IActionResult> AddRelayScenarion([FromBody] RelayScenarioDTO relayScenario)
        {
            try
            {
                var result = await _service.AddRelayScenarion(relayScenario);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            };
        }       

    }
}
