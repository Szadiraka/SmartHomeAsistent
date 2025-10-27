using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Text.Json;

namespace SmartHomeAsistent.Entities
{
    public class TuyaDbContext: DbContext
    {

        public TuyaDbContext(DbContextOptions<TuyaDbContext> options) : base(options) 
        { } 


        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<UserDevice> UserDevices { get; set; }
        public DbSet<DeviceLog> DeviceLogs { get; set; }
        public DbSet<RelayScenario> RelayScenarios { get; set; }
        public DbSet<RelayCommand> RelayCommands { get; set; } 
        //public DbSet<RepeatSettings> RepeatSettings { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserDevice>()
                .HasKey(ud => new { ud.UserId, ud.DeviceId });

            
            modelBuilder.Entity<Account>()
                .HasOne(a => a.Owner)
                .WithMany(u => u.Accounts)
                .HasForeignKey(a => a.OwnerId)
                .OnDelete(DeleteBehavior.Restrict); 

           
            modelBuilder.Entity<Account>()
                .HasMany(a => a.SharedUsers)
                .WithMany();


            //  RepeatSettings как owned тип (JSON)


            var dayOfWeekComparer = new ValueComparer<List<DayOfWeek>>(
                (c1, c2) => c1.SequenceEqual(c2),
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                c => c.ToList()
            );

            var dateTimeComparer = new ValueComparer<List<DateTime>>(
                (c1, c2) => c1.SequenceEqual(c2),
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                c => c.ToList()
            );



            modelBuilder.Entity<RelayCommand>()
              .OwnsOne(rc => rc.RepeatSettings, rs =>
              {
                  // Настройка хранения DaysOfWeek как JSON
                  rs.Property(r => r.DaysOfWeek)
                    .HasColumnName("DaysOfWeek")
                    .HasConversion(
                      v => v == null ? null : JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                      v => v == null ? null : JsonSerializer.Deserialize<List<DayOfWeek>>(v, (JsonSerializerOptions)null)
                    )
                    .Metadata.SetValueComparer(dayOfWeekComparer);

                  // Настройка хранения SpecificDates как JSON
                  rs.Property(r => r.SpecificDates)
                    .HasColumnName("SpecificDates")
                    .HasConversion(
                      v => v == null ? null : JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                      v => v == null ? null : JsonSerializer.Deserialize<List<DateTime>>(v, (JsonSerializerOptions)null)
                    ).Metadata.SetValueComparer(dateTimeComparer);
              });

          

           






        }
    }
}
