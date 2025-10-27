using Microsoft.EntityFrameworkCore;
using SmartHomeAsistent.CustomExceptions;
using SmartHomeAsistent.DTO;
using SmartHomeAsistent.Entities;
using SmartHomeAsistent.services.interfaces;
using System.Runtime.CompilerServices;

namespace SmartHomeAsistent.services.classes
{
    public class RelayScenarioService : IRelayScenarioService
    {
        private readonly TuyaDbContext _context;

        public RelayScenarioService(TuyaDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddRelayScenarion(RelayScenarioDTO relayScenario)
        {
            
            var relayScenarion = new RelayScenario()
            {
                Name = relayScenario.Name,
                UserId = relayScenario.UserId
            };
            await _context.RelayScenarios.AddAsync(relayScenarion);
           await _context.SaveChangesAsync();
            return true;

        }

        public async Task<bool> DeleteRelayScenario(int relayScenarioId, int userId)
        {
            RelayScenario relayScenarion = await _context.RelayScenarios.FirstOrDefaultAsync(x => x.Id == relayScenarioId)
                ?? throw new NotFoundException("Сценарии не найдены");

            if(relayScenarion.UserId != userId)
                throw new UnauthorizedException("Увас нет прав на удаление сценария другого пользователя");

            _context.RelayScenarios.Remove(relayScenarion);
            await _context.SaveChangesAsync();
            return true;

        }

        public async Task<List<RelayScenario>> GetAllRelayScenariosByUserId(int userId)
        {
           List<RelayScenario> relayScenarios = await _context.RelayScenarios.Where(x => x.UserId == userId).ToListAsync();
            return relayScenarios;
        }

        public async Task<RelayScenario> GetRelayScenarioById(int id)
        {
           RelayScenario scenario = await _context.RelayScenarios.FirstOrDefaultAsync(x => x.Id == id)
                ?? throw new NotFoundException("Сценарий не найден");
            return scenario;
        }

        public async Task<bool> UpdateRelayScenario(int relayScenarioId, RelayScenarioDTO relayScenario)
        {
           RelayScenario scenario = await _context.RelayScenarios.FirstOrDefaultAsync(x => x.Id == relayScenarioId)
                ?? throw new NotFoundException("Сценарий не найден");

            if(scenario.UserId != relayScenario.UserId)
                throw new UnauthorizedException("У вас нет прав на редактирование сценария другого пользователя");

            scenario.Name = relayScenario.Name;

            _context.RelayScenarios.Update(scenario);
            await _context.SaveChangesAsync();
            return true;

        }
    }
}
