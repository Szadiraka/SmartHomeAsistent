using Microsoft.EntityFrameworkCore;
using SmartHomeAsistent.DTO;
using SmartHomeAsistent.Entities;
using SmartHomeAsistent.services.interfaces;
using System;
using System.Linq.Expressions;

namespace SmartHomeAsistent.services.classes
{
    public class DeviceService : IDeviceService
    {
        private readonly TuyaDbContext _context;

        public DeviceService(TuyaDbContext dbContext)
        {
            _context = dbContext;
        }


        public async Task<bool> AddDeviceAsync(DeviceDTO deviceDto)
        {
           Account account = _context.Accounts.FirstOrDefault(x=>x.Id == deviceDto.AccountId) ?? throw new Exception("Аккаунт с указанным id не найден");
           if(await _context.Devices.AnyAsync(x => x.DeviceUniqueId == deviceDto.DeviceUniqueId))
               throw new Exception("Устройство с таким уникальным id уже существует");
            Device device = new()
            {
                Name = deviceDto.Name,
                AccountId = deviceDto.AccountId,
                Account = account,
                DeviceUniqueId = deviceDto.DeviceUniqueId,
                SwitchingPower = deviceDto.SwitchingPower ?? 0

            };
            await _context.Devices.AddAsync(device);
            await _context.SaveChangesAsync();
            return true;

        }

        public async Task<bool> DeleteDeviceAsync(int id)
        {
           Device device = _context.Devices.FirstOrDefault(x => x.Id == id) ?? throw new Exception("Устройство с указанным id не найдено");
           _context.Devices.Remove(device);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Device>> GetAllDevices(Expression<Func<Device, bool>> predicate)
        {
           var devices =await  _context.Devices.Where(predicate).ToListAsync();
            return devices;
        }

        public async Task<Device> GetDeviceById(int id)
        {
            Device device =await _context.Devices.FirstOrDefaultAsync(x=>x.Id == id) ?? throw new Exception("Устройство с указанным id не найдено");
            return device;
        }

        public async Task<List<Device>> GetDevicesByAccountId(int accountId)
        {
           var devices = await _context.Devices.Where(x => x.AccountId == accountId).ToListAsync();
            return devices;
        }

        public async Task<bool> UpdateDeviceAsync(int id, DeviceDTO deviceDto)
        {
            Device device = await _context.Devices.FirstOrDefaultAsync(x => x.Id == id) ?? throw new Exception("Устройство с указанным id не найдено");
       
            device.Name = deviceDto.Name;
            device.AccountId = deviceDto.AccountId;
            device.DeviceUniqueId = deviceDto.DeviceUniqueId;
            device.SwitchingPower = deviceDto.SwitchingPower ?? device.SwitchingPower;

            await _context.Devices.AddAsync(device);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
