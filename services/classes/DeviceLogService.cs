using Microsoft.EntityFrameworkCore;
using SmartHomeAsistent.DTO;
using SmartHomeAsistent.Entities;
using SmartHomeAsistent.services.interfaces;
using System.Runtime.CompilerServices;

namespace SmartHomeAsistent.services.classes
{
    public class DeviceLogService : IDeviceLogService
    {
        private readonly TuyaDbContext _context;

        public DeviceLogService(TuyaDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddDeviceLog(DeviceLogDTO deviceLogDTO)
        {
            DeviceLog deviceLOg = new()
            {
                DeviceId = deviceLogDTO.DeviceId,
                IsOn = deviceLogDTO.IsOn,
                TimeStamp = DateTime.UtcNow
            };

            await _context.DeviceLogs.AddAsync(deviceLOg);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteDeviceLog(int id)
        {
           DeviceLog deviceLog =await _context.DeviceLogs.FirstOrDefaultAsync(x=>x.Id == id) 
                ?? throw new Exception("девай-лог не найден");

            _context.DeviceLogs.Remove(deviceLog);
            await _context.SaveChangesAsync();
            return true;

        }

        public async Task<DeviceLog> GetDeviceLogById(int id)
        {
            DeviceLog deviceLog = await _context.DeviceLogs.FirstOrDefaultAsync(x => x.Id == id)
               ?? throw new Exception("девай-лог не найден");
            return deviceLog;
        }

        public async Task<List<DeviceLog>> GetDevicesLogsByDeviceId(int deviceId, DateTime? from, DateTime? to)
        {
            var query = _context.DeviceLogs.Where(x=>x.DeviceId == deviceId);
      

            if(from.HasValue)
                query = query.Where(x => x.TimeStamp >= from);
            if(to.HasValue)
                query = query.Where(x => x.TimeStamp <= to);

            query = query.OrderByDescending(x => x.TimeStamp);

            return await query.ToListAsync();
        }

        public async Task<List<DeviceLog>> GetAllDeviceLogs(DateTime? from, DateTime? to)
        {
            var query = _context.DeviceLogs.AsQueryable();

            if (from.HasValue)
                query = query.Where(x => x.TimeStamp >= from);
            if (to.HasValue)
                query = query.Where(x => x.TimeStamp <= to);

            query = query.OrderByDescending(x => x.TimeStamp);

            return await query.ToListAsync();
        }

        public async Task<bool> UpdateDeviceLog(int id, DeviceLogDTO deviceLogDTO)
        {
            DeviceLog deviceLog = await _context.DeviceLogs.FirstOrDefaultAsync(x => x.Id == id)
                 ?? throw new Exception("девайс-лог не найден");
            deviceLog.DeviceId = deviceLogDTO.DeviceId;
            deviceLog.IsOn = deviceLogDTO.IsOn;
            deviceLog.TimeStamp = DateTime.UtcNow;

            _context.DeviceLogs.Update(deviceLog);
            _context.SaveChanges();
            return true;
        }
    }
}
