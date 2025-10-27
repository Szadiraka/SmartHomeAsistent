using SmartHomeAsistent.DTO;
using SmartHomeAsistent.Entities;

namespace SmartHomeAsistent.services.interfaces
{
    public interface IAccountService
    {

        Task<bool> AddAccountAsync(AccountDTO accountDto);

        Task<bool> UpdateAccountAsync(int id, AccountDTO accountDto);

        Task<Account> GetAccountById(int id);

        Task<List<Account>> GetAccountsByUserId(int userId);

        Task<List<Account>> GetAllAccounts();       

        Task<bool> DeleteAccountAsync(int id);

        Task<bool> AddSecondaryUserToAccountAsync(SecondaryUserDTO secondaryUserDto);
    }
}
