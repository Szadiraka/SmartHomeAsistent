using SmartHomeAsistent.DTO;

namespace SmartHomeAsistent.services.interfaces
{
    public interface IRelayService
    {

        Task<ResponseElementDTO> SwitchRelayAsync(string id, string action);


        Task<ResponseElementDTO> GetStatusAsync(string id);

    }
}
