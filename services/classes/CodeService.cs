using Microsoft.EntityFrameworkCore;
using SmartHomeAsistent.CustomExceptions;
using SmartHomeAsistent.Entities;
using SmartHomeAsistent.services.interfaces;
using System.Linq.Expressions;

namespace SmartHomeAsistent.services.classes
{
    public class CodeService : ICodeService
    {
        private readonly TuyaDbContext _context;

        public CodeService(TuyaDbContext context)
        {
            _context = context;
        }
        public async Task<bool> CreateCode(int userId, int code, int expirationTime)
        {
            Code curentCode = new Code
            {
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddMinutes(expirationTime),
                Data = code
            };
            await _context.AddAsync(curentCode);
            await _context.SaveChangesAsync();
            return true;


        }

        public async Task<Code> GetLastCodeByUserId(int userId)
        {
            Code code = await _context.Codes.Where(x=>x.UserId == userId)
                .OrderByDescending(x=>x.CreatedAt)
                .FirstOrDefaultAsync() ?? throw new NotFoundException("Код не найден");
            return code;
        }

        public async Task<bool> DeleteCode(int id)
        {
            Code code = await _context.Codes.FirstOrDefaultAsync(x=>x.Id == id) ?? throw new NotFoundException("Код не найден");
            _context.Codes.Remove(code);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Code>> GetAllCodesByUserId(int userId, Expression<Func<Code, bool>>? predicate)
        {
            var query = _context.Codes.Where(x => x.UserId == userId).AsQueryable();
            if (predicate != null)
            {
                query = query.Where(predicate);
            }
            return await query.ToListAsync();
        }

        public async Task<Code> GetCodeById(int id)
        {
            Code code = await _context.Codes.FirstOrDefaultAsync(x => x.Id == id) ?? throw new NotFoundException("Код не найден");
            return code;
        }

        public async Task<bool> UpdateCode(int id, int expirationTime)
        {
            Code code = await _context.Codes.FirstOrDefaultAsync(x => x.Id == id) ?? throw new NotFoundException("Код не найден");
        
            code.ExpiresAt= DateTime.UtcNow.AddMinutes(expirationTime);

            _context.Update(code);
            await  _context.SaveChangesAsync();
            return true;
         }

        public async Task<bool> ConfirmCodeAsync(int userId, int code)
        {
            Code lastCode = await _context.Codes
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.CreatedAt)
                .FirstOrDefaultAsync() ?? throw new NotFoundException("Код не найден");
            if (DateTime.UtcNow > lastCode.ExpiresAt)
                throw new ValidationException("Код устарел");
            if (code != lastCode.Data)
                throw new ValidationException("Код неверен");
            return true;

        }
    }
}
