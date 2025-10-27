using SmartHomeAsistent.DTO;
using SmartHomeAsistent.Entities;
using System.Linq.Expressions;

namespace SmartHomeAsistent.services.interfaces
{
    public interface IDeviceService
    {
        Task<bool> AddDeviceAsync(DeviceDTO deviceDto);

        Task<bool> UpdateDeviceAsync(int id, DeviceDTO deviceDto);

        Task<Device> GetDeviceById(int id);

        Task<List<Device>> GetDevicesByAccountId(int accountId);

        Task<List<Device>> GetAllDevices(Expression<Func<Device, bool>> predicate);

        Task<bool> DeleteDeviceAsync(int id);

    }
}
