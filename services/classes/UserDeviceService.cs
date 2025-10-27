using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using SmartHomeAsistent.DTO;
using SmartHomeAsistent.Entities;
using SmartHomeAsistent.services.interfaces;
using System.Data;

namespace SmartHomeAsistent.services.classes
{
    public class UserDeviceService : IUserDeviceService
    {
        private readonly TuyaDbContext _context;

        public UserDeviceService(TuyaDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddUserDevice(UserDeviceDTO userDeviceDTO)
        {
            if(_context.UserDevices.Any(x=>x.UserId == userDeviceDTO.UserId && x.DeviceId == userDeviceDTO.DeviceId))
                throw new Exception("Такое устройство уже привязано к пользователю");
            User user =await _context.Users.FirstOrDefaultAsync(x => x.Id == userDeviceDTO.UserId && x.Hidden == false && x.IsBlocked == false)
                ?? throw new Exception("Пользователь не найден");
            Device device =await _context.Devices.FirstOrDefaultAsync(x => x.Id == userDeviceDTO.DeviceId )
                ?? throw new Exception("Устройство не найдено");

            UserDevice userDevice = new()
            {
                UserId = userDeviceDTO.UserId,
                DeviceId = userDeviceDTO.DeviceId,
                Level = userDeviceDTO.Level,
                ProvidedAt = DateTime.UtcNow,
            };
            await _context.UserDevices.AddAsync(userDevice);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteUserDevice(int userId, int deviceId)
        {
            UserDevice userDevice = await _context.UserDevices.FirstOrDefaultAsync(x => x.UserId == userId && x.DeviceId == deviceId)
                ?? throw new Exception("Сущность не найдена");

            _context.UserDevices.Remove(userDevice);
            await _context.SaveChangesAsync();
            return true;

        }

        public async Task<UserDevice> GetUserDeviceById(int userId, int deviceId)
        {
            UserDevice userDevice = await _context.UserDevices.FirstOrDefaultAsync(x => x.UserId == userId && x.DeviceId == deviceId)
                ?? throw new Exception("Сущность не найдена");

            return userDevice;
        }

        public async Task<List<UserDevice>> GetUserDevicesByDeviceId(int deviceId)
        {
            var userDevices =await _context.UserDevices.Where(x => x.DeviceId == deviceId).ToListAsync();
            return userDevices;
        }

        public async Task<List<UserDevice>> GetUserDevicesByUserId(int userId)
        {
            var userDevices = await _context.UserDevices.Where(x => x.UserId == userId).ToListAsync();
            return userDevices;
        }

        public async Task<bool> UpdateUserDevice(int userId, int deviceId, UserDeviceDTO userDeviceDTO)
        {
            UserDevice userDevice = await _context.UserDevices.FirstOrDefaultAsync(x => x.UserId == userId && x.DeviceId == deviceId)
               ?? throw new Exception("Сущность не найдена");

            userDevice.UserId = userDeviceDTO.UserId;
            userDevice.DeviceId = userDeviceDTO.DeviceId;
            userDevice.Level = userDeviceDTO.Level;
            userDevice.ProvidedAt = DateTime.UtcNow;

            _context.UserDevices.Update(userDevice);
            await _context.SaveChangesAsync();
            return true;

        }
    }
}
