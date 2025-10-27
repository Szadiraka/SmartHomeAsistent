using SmartHomeAsistent.DTO;
using SmartHomeAsistent.Entities;

namespace SmartHomeAsistent.services.interfaces
{
    public interface IRelayCommandService
    {
        Task <bool> AddRelayCommand(RelayCommandDTO relayCommand);

        Task<List<RelayCommand>> GetRelayCommandsByRelayScenarioId(int relayscenarioId);

        Task<RelayCommand> GetRelayCommandById(int id);

        Task<bool> DeleteRelayCommand(int relayCommandId, int userId);

        Task<bool> UpdateRelayCommand(int relayCommandId, int userId, RelayCommandDTO relayCommand);

    }
}
