using SmartHomeAsistent.DTO;
using SmartHomeAsistent.Entities;

namespace SmartHomeAsistent.services.interfaces
{
    public interface IUserDeviceService
    {

        Task <bool> AddUserDevice(UserDeviceDTO userDeviceDTO);

        Task <bool> UpdateUserDevice(int userid,int deviceId, UserDeviceDTO userDeviceDTO);

        Task <bool> DeleteUserDevice(int userid,int deviceId);

        Task <UserDevice> GetUserDeviceById(int userid, int deviceId);

        Task <List<UserDevice>> GetUserDevicesByUserId(int userId);

        Task<List<UserDevice>> GetUserDevicesByDeviceId(int deviceId);

    }
}


