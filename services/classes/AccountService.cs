using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SmartHomeAsistent.CustomExceptions;
using SmartHomeAsistent.DTO;
using SmartHomeAsistent.Entities;
using SmartHomeAsistent.services.interfaces;

namespace SmartHomeAsistent.services.classes
{
    public class AccountService : IAccountService
    {
        private readonly TuyaDbContext _context;
        private readonly EncryptionService _encryptionService;

        public AccountService(TuyaDbContext context, EncryptionService encryptionService)
        {
            _context = context;
            _encryptionService = encryptionService;
           
        }


        public async Task<bool> AddAccountAsync(AccountDTO accountDto)
        {

            User user = _context.Users.FirstOrDefault(x => x.Id == accountDto.OwnerId) 
                ?? throw new NotFoundException("Пользователь с указанным id не найден");
            Account account = new()
            {
                Name = accountDto.Name.ToLower(),
                OwnerId = accountDto.OwnerId,
                Owner = user,
                AccessKey = _encryptionService.Encrypt(accountDto.AccessKey),
                SecretKey = _encryptionService.Encrypt(accountDto.SecretKey)
            };
           

            await _context.Accounts.AddAsync(account);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAccountAsync(int id)
        {
            if (id <= 0)
                throw new ValidationException("Невалидный Id аккаунта");

            Account account = _context.Accounts.FirstOrDefault(x => x.Id == id)
                ?? throw new NotFoundException("Аккаунт с указанным id не найден");
           
          if(await _context.Devices.AnyAsync(x => x.AccountId == id))
                throw new BadRequestException("Вы не можете удалить аккаунт, т.к. у данного аккаунта есть устройства");

            _context.Accounts.Remove(account);
            _context.SaveChanges();
            return true;
        }

        public async Task<Account> GetAccountById(int id)
        {
            if (id <= 0)
                throw new ValidationException("Невалидный Id аккаунта");
            Account account =await _context.Accounts.FirstOrDefaultAsync(x => x.Id == id) 
                ?? throw new NotFoundException("Аккаунт с указанным id не найден");
            return account;
        }

        public async Task<List<Account>> GetAccountsByUserId(int userId)
        {
            if (userId <= 0)
                throw new ValidationException("Невалидный Id пользователя");

            var accounts = await _context.Accounts.Where(x => x.OwnerId == userId).ToListAsync();           
            return accounts;
        }

        public async Task<List<Account>> GetAllAccounts()
        {
           var accounts =await  _context.Accounts.ToListAsync();           
            return accounts;
        }

        public async Task<bool> UpdateAccountAsync(int id, AccountDTO accountDto)
        {
            if (id <= 0)
                throw new ValidationException("Невалидный Id аккаунта");

            Account account = _context.Accounts.FirstOrDefault(x => x.Id == id) 
                ?? throw new NotFoundException("Аккаунт с указанным id не найден");


            account.AccessKey = _encryptionService.Encrypt(accountDto.AccessKey);
            account.SecretKey = _encryptionService.Encrypt(accountDto.SecretKey);
            account.Name = accountDto.Name;

            _context.Accounts.Update(account);
            await _context.SaveChangesAsync();
            return true;


        }

        public async Task<bool> AddSecondaryUserToAccountAsync(SecondaryUserDTO secondaryUserDto)
        {
            User owner = await _context.Users.FirstOrDefaultAsync(x => x.Id == secondaryUserDto.OwnerId && x.Hidden != true && x.IsBlocked != true)
                ?? throw new NotFoundException("Собственник аккаунта не найден");

            Account account = await _context.Accounts.Include(x=>x.SharedUsers)
                .FirstOrDefaultAsync(x => x.Id == secondaryUserDto.AccountId) ?? throw new NotFoundException("Aккаунт не найден");

            if (account.OwnerId != secondaryUserDto.OwnerId) 
                throw new UnauthorizedException("У Вас не достаточно прав");

            if (account.SharedUsers.Any(x => x.Id == secondaryUserDto.SecondaryUserId))
                 throw new BadRequestException("Пользователь  добавлен ранее");
            User secondary = await _context.Users.FirstOrDefaultAsync(x => x.Id == secondaryUserDto.SecondaryUserId  && x.Hidden !=true && x.IsBlocked != true) 
                ?? throw new NotFoundException("Пользователь не найден");

            account.SharedUsers.Add(secondary);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
