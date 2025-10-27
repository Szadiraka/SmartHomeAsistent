using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using SmartHomeAsistent.CustomExceptions;
using SmartHomeAsistent.DTO;
using SmartHomeAsistent.Entities;
using SmartHomeAsistent.services.interfaces;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace SmartHomeAsistent.services.classes
{
    public class RelayCommandService : IRelayCommandService
    {
        private readonly TuyaDbContext _context;

        public RelayCommandService(TuyaDbContext context)
        {
            _context = context;
        }   


        public async Task<bool> AddRelayCommand(RelayCommandDTO relayCommand)
        {
            var device =await _context.Devices.FirstOrDefaultAsync(x => x.Id == relayCommand.DeviceId) 
                ?? throw new NotFoundException("Устройство с указанным id не найдено");

            var scenario = await _context.RelayScenarios.FirstOrDefaultAsync(x => x.Id == relayCommand.RelayScenarioId)
                ?? throw new NotFoundException("Сценарии не найдены");
          
            var command = CreateOrUpdateRelayCommand(relayCommand, null);

            await _context.RelayCommands.AddAsync(command);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteRelayCommand(int relayCommandId, int userId)
        {
           var command =await _context.RelayCommands.Include(x=>x.Scenario)
                .FirstOrDefaultAsync(x => x.Id == relayCommandId)
                ?? throw new NotFoundException("Команда не найдены");

           if(command.Scenario!.UserId != userId) 
                throw new UnauthorizedException("У вас нет прав на удаление команд другого пользователя");


            _context.RelayCommands.Remove(command);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<RelayCommand> GetRelayCommandById(int id)
        {
           RelayCommand command =await  _context.RelayCommands.FirstOrDefaultAsync(x => x.Id == id)
                ?? throw new NotFoundException("Команды не найдены");
            return command;
        }

        public async Task<List<RelayCommand>> GetRelayCommandsByRelayScenarioId(int relayscenarioId)
        {
            var result = await _context.RelayCommands
                         .Where(x => x.RelayScenarioId == relayscenarioId).ToListAsync();
            return result;
        }

        public async Task<bool> UpdateRelayCommand(int relayCommandId, int userId, RelayCommandDTO relayCommand)
        {
           var command = await _context.RelayCommands.Include(x=>x.Scenario)
                .FirstOrDefaultAsync(x=>x.Id == relayCommandId)
                ?? throw new NotFoundException($"Команда не найдена");

            var device = await _context.RelayCommands.FirstOrDefaultAsync(x => x.DeviceId == relayCommand.DeviceId)
                 ?? throw new NotFoundException("устройство не найдено");
            var scenario = await _context.RelayScenarios.FirstOrDefaultAsync(x => x.Id == relayCommand.RelayScenarioId)
                ?? throw new NotFoundException("Сценарий не найден");

            if (userId != scenario.UserId)
                throw new UnauthorizedException("У вас нет прав на редактирование команд другого пользователя");

            // пока сделано так, что пользователь может редактировать только свои команды
            command = CreateOrUpdateRelayCommand(relayCommand, command);          

            _context.RelayCommands.Update(command);
            await _context.SaveChangesAsync();
            return true;

        }


        private static RelayCommand CreateOrUpdateRelayCommand (RelayCommandDTO relayCommandDTO,RelayCommand? relayCommand )
        {
            if(relayCommand == null)
                relayCommand = new RelayCommand();

            relayCommand.RelayScenarioId = relayCommandDTO.RelayScenarioId;
            relayCommand.CommandName = relayCommandDTO.CommandName;
            relayCommand.DeviceId = relayCommandDTO.DeviceId;
            relayCommand.CommandType = relayCommandDTO.CommandType;
            relayCommand.SheduleType = relayCommandDTO.SheduleType;
            relayCommand.StartTime = relayCommandDTO.StartTime;
            relayCommand.Delay = relayCommandDTO.Delay;
            relayCommand.RepeatSettings = relayCommandDTO.RepeatSettings;

            return relayCommand;
        }
    }
}
