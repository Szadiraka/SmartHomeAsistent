using SmartHomeAsistent.DTO;
using SmartHomeAsistent.Entities;
using SmartHomeAsistent.Infrastructure;

namespace SmartHomeAsistent.services.interfaces
{
    public interface IRoleService
    {

        Task <bool> AddRoleAsync(string roleName);

        Task<bool> UpdateRoleAsync(int id, string newRoleName);     

        Task<Role> GetRoleById(int id);

        Task<Role> GetRoleByName(string roleName);   
        
        Task<List<Role>> GetAllRoles();

       
    }
}
