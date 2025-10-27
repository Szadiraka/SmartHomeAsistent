using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SmartHomeAsistent.DTO;
using SmartHomeAsistent.Entities;
using SmartHomeAsistent.Infrastructure;
using SmartHomeAsistent.services.interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;

namespace SmartHomeAsistent.services.classes
{
    public class UserService : IUserService
    {
        private readonly TuyaDbContext _context;
        private readonly PasswordHasher<User> passwordHasher;
        private readonly IConfiguration _configuration;

        public UserService(TuyaDbContext context, IConfiguration configuration)
        {
            _context = context;
            passwordHasher = new PasswordHasher<User>();
            _configuration = configuration;

        }


        public async Task<User> RegisterAsync(UserDTO userDto)
        {
            if(_context.Users.Any(x=>x.Email == userDto.Email))
                throw new Exception("Пользователь с такой почтой уже существует");

            var roleName = userDto.RoleName ?? "client";
            var role = _context.Roles.FirstOrDefault(x => x.Name == roleName);
            if (role == null)
                throw new Exception("Такой роли не существует");
            var user = new User
            {
                Name = userDto.Name,
                Email = userDto.Email,
                IsBlocked = false,
                RoleId = role.Id
            };

            user.PasswordHash = passwordHasher.HashPassword(user, userDto.Password);
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;


        }

        public async Task<string> Login(string email, string password)
        {
            
            var user =await _context.Users.FirstOrDefaultAsync(x => x.Email == email && x.Hidden ==false);
            if ( user == null)
                throw new Exception("Пользователя с такой почтой не существует");

            if (user.IsBlocked)
                throw new Exception("Пользователь заблокирован");

             var result = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
            if(result == PasswordVerificationResult.Failed)
                throw new Exception("Неверный пароль");

            // после всех успешных проверок, создаем токен и возвращаем пользователя

            var claims = new List<Claim> 
            {  new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Role, user.Role.Name)
            };

            JwtSetting? jwtSettings =_configuration.GetSection("JwtSettings").Get<JwtSetting>();
            if(jwtSettings == null)
                throw new Exception("Командная строка не нашлась");
                
            var jwt = new JwtSecurityToken(
                issuer: jwtSettings.Issuer,
                audience: jwtSettings.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(jwtSettings.DurationInMinutes),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),
                    SecurityAlgorithms.HmacSha256
                )

            );
            var encodeJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return encodeJwt;

        }


        public async Task<List<User>> GetAllUsersAsync(UserFilter? filter)
        {
           IQueryable<User> query = _context.Users;
         
            if(filter != null)
            {
                if (!string.IsNullOrWhiteSpace(filter.Name))
                    query = query.Where(x => x.Name.ToLower().Contains(filter.Name.ToLower()));

                if (!string.IsNullOrWhiteSpace(filter.Email))
                    query = query.Where(x => x.Email.ToLower().Contains(filter.Email.ToLower()));

                if (!string.IsNullOrWhiteSpace(filter.RoleName))
                    query = query.Where(x => x.Role.Name.ToLower().Contains(filter.RoleName.ToLower()));
                if (filter.IsBlocked != null)
                    query = query.Where(x => x.IsBlocked == filter.IsBlocked);
                if (filter.Hidden != null)
                    query = query.Where(x => x.Hidden == filter.Hidden);
            }
            return await query.ToListAsync();
        }


        public async Task<bool> UpdateUserAsync(int id, UserDTO userDto)
        {
            User? user = _context.Users.FirstOrDefault(x => x.Id == id);
            if (user == null)
               throw new Exception("Пользователь не найден");
            user.Name = userDto.Name;
            user.PasswordHash = passwordHasher.HashPassword(user, userDto.Password);
            if(userDto.Email != userDto.Email)
            {
               bool isExist = await _context.Users.AnyAsync(x => x.Email == userDto.Email && x.Id != id);
                if (isExist)
                  throw new Exception("Пользователь с такой почтой уже существует");

                user.Email = userDto.Email;
            }

            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<bool> BlockUserAsync(int id)
        {
           User? user = _context.Users.FirstOrDefault(x => x.Id == id);
            if(user == null)
                throw new Exception("Пользователь не найден");
            if(user.IsBlocked == true)
                return true;
            user.IsBlocked = true;
            _context.Users.Update(user);
            await  _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UnblockUserAsync(int id)
        {
            User? user = _context.Users.FirstOrDefault(x => x.Id == id);
            if (user == null)
                throw new Exception("Пользователь не найден");
            if (user.IsBlocked == false)
                return true;

            user.IsBlocked = false;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<User> GetUserById(int id)
        {
            User? user =await _context.Users.FirstOrDefaultAsync(x => x.Id == id && x.Hidden == false);
            if (user == null)
                throw new Exception("Пользователь не найден");
            return user;
        }
       

        public async Task<bool> DeleteUserAsync(int id)
        {
            User? user = _context.Users.FirstOrDefault(x => x.Id == id && x.Hidden == false);
            if (user == null)
                throw new Exception("Пользователь не найден");
           user.Hidden = true;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RecoveryUserAsync(int id)
        {
            User? user = _context.Users.FirstOrDefault(x => x.Id == id );
            if (user == null)
                throw new Exception("Пользователь не найден");
            if(user.Hidden)
                return true;

            user.Hidden = false;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
