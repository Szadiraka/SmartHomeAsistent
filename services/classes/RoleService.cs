using Microsoft.EntityFrameworkCore;
using SmartHomeAsistent.CustomExceptions;
using SmartHomeAsistent.Entities;
using SmartHomeAsistent.services.interfaces;
using System.Runtime.CompilerServices;

namespace SmartHomeAsistent.services.classes
{
    public class RoleService : IRoleService
    {
        private readonly TuyaDbContext _context;

        public RoleService(TuyaDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddRoleAsync(string roleName)
        {
            if(string.IsNullOrEmpty(roleName))
                throw new ValidationException("Некорректные название роли");

            Role role = new Role
            {
                Name = roleName
            };
            await _context.Roles.AddAsync(role);
            await _context.SaveChangesAsync();
            return true;

        }

        public async Task<List<Role>> GetAllRoles()
        {
            var roles =await  _context.Roles.ToListAsync();
            return roles;
        }

        public async Task<Role> GetRoleById(int id)
        {
           Role role =await _context.Roles.FirstOrDefaultAsync(x => x.Id == id)
                ?? throw new NotFoundException("Роли с таким id не существует");           
               
            return role;
        }


        public async Task<Role> GetRoleByName(string roleName)
        {
            Role role =await _context.Roles.FirstOrDefaultAsync(x => x.Name.ToLower() == roleName.ToLower())
                ?? throw new NotFoundException("Роли с таким именем не существует");
            return role;
        }

        public async Task<bool> UpdateRoleAsync(int id, string newRoleName)
        {
            if(string.IsNullOrEmpty(newRoleName))
                throw new ValidationException("Некорректные название роли");

           Role role = _context.Roles.FirstOrDefault(x=>x.Id == id)
                 ?? throw new NotFoundException("Роль с указанным Id не существует");
            if (role.Name.ToLower() == newRoleName.ToLower())
               throw new BadRequestException("Роль с таким именем уже существует");
            role.Name = newRoleName.ToLower();
            _context.Roles.Update(role);
            await _context.SaveChangesAsync();

            return true;

        }

    }
}
