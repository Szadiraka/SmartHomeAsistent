using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SqlServer.Server;
using SmartHomeAsistent.DTO;
using SmartHomeAsistent.Entities;
using SmartHomeAsistent.Infrastructure;
using System.Linq.Expressions;

namespace SmartHomeAsistent.services.interfaces
{
    public interface IUserService
    {

        Task <User> RegisterAsync(UserDTO userDto);

        Task<AnswerDTO> Login(string email, string password);

        Task <bool> UpdateUserAsync(int id, UserDTO userDto);

        Task <bool> BlockUserAsync(int id);

        Task <bool> UnblockUserAsync(int id);

        Task <User> GetUserById(int id);

        Task <List<User>> GetAllUsersAsync(UserFilter? filter);

        Task <bool> DeleteUserAsync(int id);

        Task<bool> RecoveryUserAsync(int id);


    }
}
