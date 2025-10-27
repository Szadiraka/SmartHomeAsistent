using SmartHomeAsistent.DTO;
using SmartHomeAsistent.Entities;

namespace SmartHomeAsistent.services.interfaces
{
    public interface IDeviceLogService
    {


        public Task<bool> AddDeviceLog(DeviceLogDTO deviceLogDTO);

        public Task<bool> DeleteDeviceLog(int id);

        public Task<bool> UpdateDeviceLog(int id, DeviceLogDTO deviceLogDTO);

        public Task<DeviceLog> GetDeviceLogById(int id);

        public Task<List<DeviceLog>> GetDevicesLogsByDeviceId(int deviceId, DateTime? from,DateTime? to );

        public Task<List<DeviceLog>> GetAllDeviceLogs(DateTime? from, DateTime? to);
    }
}
