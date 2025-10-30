using Microsoft.Extensions.Configuration.UserSecrets;
using SmartHomeAsistent.DTO;
using SmartHomeAsistent.Entities;
using System.Linq.Expressions;

namespace SmartHomeAsistent.services.interfaces
{
    public interface ICodeService
    {

        Task<bool> CreateCode(int userId, int code, int expirationTime);
        Task<bool> DeleteCode(int id);
        Task<Code> GetCodeById(int id);
        Task <IEnumerable<Code>> GetAllCodesByUserId(int userId, Expression<Func<Code, bool>>? predicate);
        Task<bool> UpdateCode(int id, int expirationTime);
        Task<bool> ConfirmCodeAsync(int userId, int code);
        Task<Code> GetLastCodeByUserId(int userId);
    }
}
