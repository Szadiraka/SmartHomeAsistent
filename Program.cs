using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SmartHomeAsistent.Entities;
using SmartHomeAsistent.middleware;
using SmartHomeAsistent.services;
using SmartHomeAsistent.services.classes;
using SmartHomeAsistent.services.interfaces;
using SmartHomeAsistent.signalR;
using System.Text;

namespace SmartHomeAsistent
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // добавляем сигналR в сервисы
            builder.Services.AddSignalR();

            // добавляем CORS политику
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowClient", policy =>
                {
                    policy
                        .WithOrigins("http://localhost:4100") 
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials(); 
                });
            });

            var jwtSettings = builder.Configuration.GetSection("JwtSettings");

            builder.Services.AddAuthorization();
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSettings.GetSection("Issuer").Value,
                        ValidAudience = jwtSettings.GetSection("Audience").Value,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.GetSection("SecretKey").Value ?? string.Empty))
                    };
                });



            string dbConnectionString = builder.Configuration.GetSection("ConnectionStrings:DbConnectionString").Value ?? throw new Exception("командная стока не найдена");
            builder.Services.AddDbContext<TuyaDbContext>(op=>op.UseSqlServer(dbConnectionString));

            //подключаем hangHire SqlServer
            string hangFireConnectionString = builder.Configuration.GetSection("ConnectionStrings:HangFireConnectionString").Value ?? throw new Exception("командная строка к hangfire не найдена");
            builder.Services.AddHangfire(config =>config                
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(hangFireConnectionString));

            //Запускаем сервер Hangfire
            builder.Services.AddHangfireServer();


            builder.Services.AddSingleton<EncryptionService>();

            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IRoleService, RoleService>();
            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddScoped<IDeviceService, DeviceService>();
            builder.Services.AddScoped<IUserDeviceService, UserDeviceService>();
            builder.Services.AddScoped<IDeviceLogService, DeviceLogService>();
            builder.Services.AddScoped<IRelayScenarioService, RelayScenarioService>();
            builder.Services.AddScoped<IRelayCommandService, RelayCommandService>();
            builder.Services.AddScoped<ICodeService, CodeService>();
           
            builder.Services.AddSingleton<IMessageService,EmailService>();

            builder.Services.AddSingleton<IEventHubService ,EventHubService>();


            builder.Services.AddHttpClient<IRelayService, RelayService>();
            builder.Services.AddControllers();




            var app = builder.Build();

            //кастомный мидлвэа
            app.UseMiddleware<ErrorHadlingMiddleware>();

            // Configure the HTTP request pipeline.
            app.UseCors("AllowClient");

            app.UseHttpsRedirection();
            
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseHangfireDashboard("/hangfire");

            app.MapControllers();

            app.MapHub<EventHub>("/relayHub");

            await DbInitializer.SeedAsync(app.Services);

            app.Run();
        }
    }
}
