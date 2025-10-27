using Microsoft.EntityFrameworkCore;
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
           Role? role =await _context.Roles.FirstOrDefaultAsync(x => x.Id == id);
           if(role == null)
                throw new Exception("Роли с таким id не существует");
            return role;
        }


        public async Task<Role> GetRoleByName(string roleName)
        {
            Role? role =await _context.Roles.FirstOrDefaultAsync(x => x.Name.ToLower() == roleName.ToLower());
            if (role == null)
                throw new Exception("Роли с таким именем не существует");
            return role;
        }

        public async Task<bool> UpdateRoleAsync(int id, string newRoleName)
        {
           Role? role = _context.Roles.FirstOrDefault(x=>x.Id == id);
            if (role == null)
                throw new Exception("Роль с указанным Id не существует");
            if (role.Name.ToLower() == newRoleName.ToLower())
                return true;
            role.Name = newRoleName.ToLower();
            _context.Roles.Update(role);
            await _context.SaveChangesAsync();

            return true;

        }

    }
}
