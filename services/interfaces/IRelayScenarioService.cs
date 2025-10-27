using SmartHomeAsistent.DTO;
using SmartHomeAsistent.Entities;

namespace SmartHomeAsistent.services.interfaces
{
    public interface IRelayScenarioService
    {
        public Task<RelayScenario> GetRelayScenarioById(int id);

        public Task<List<RelayScenario>> GetAllRelayScenariosByUserId(int userId);

        public Task<bool> UpdateRelayScenario(int relayScenarioId, RelayScenarioDTO relayScenario);

        public Task<bool> DeleteRelayScenario(int relayScenarioId);

        public Task<bool> AddRelayScenarion(RelayScenarioDTO relayScenario);




    }
}
