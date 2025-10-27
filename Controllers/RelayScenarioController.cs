using Microsoft.AspNetCore.Mvc;
using SmartHomeAsistent.DTO;
using SmartHomeAsistent.Entities;

namespace SmartHomeAsistent.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RelayScenarioController : ControllerBase
    {

        public Task<RelayScenario> GetRelayScenarioById(int id);

        public Task<List<RelayScenario>> GetAllRelayScenariosByUserId(int userId);

        public Task<bool> UpdateRelayScenario(int relayScenarioId, RelayScenarioDTO relayScenario);

        public Task<bool> DeleteRelayScenario(int relayScenarioId);

        public Task<bool> AddRelayScenarion(RelayScenarioDTO relayScenario);

    }
}
