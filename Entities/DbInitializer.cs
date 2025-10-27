using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace SmartHomeAsistent.Entities
{
    public static class DbInitializer
    {

        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<TuyaDbContext>();       

            if (!db.Roles.Any())
            {
                await db.Roles.AddRangeAsync(
                    [new Role {Name = "admin" },
                     new Role {Name = "client" }
                    ]
                );
                await db.SaveChangesAsync();
            }

            if (!db.Users.Any())
            {
                var role = await db.Roles.FirstOrDefaultAsync(x => x.Name == "admin");
                if (role != null)
                {
                    var user = new User
                    {
                        Name = "admin",
                        Email = "szadiraka0509@gmail.com",
                        RoleId = role.Id

                    };
                    var passwordHasher = new PasswordHasher<User>();
                    var passwordHash = passwordHasher.HashPassword(user, "12345");
                    user.PasswordHash = passwordHash;

                    await db.Users.AddAsync(user);
                    await db.SaveChangesAsync();
                }
               
            }
        }
       
    }
}
