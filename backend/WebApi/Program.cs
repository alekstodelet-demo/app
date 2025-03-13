using Application.Repositories;
using Application.UseCases;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Sinks.Telegram;
using System.Data;
using System.Text;
using WebApi.Middleware;

namespace WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var botToken = builder.Configuration["Logging:Token"];
            var chatId = builder.Configuration["Logging:Channel"];
            
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Telegram(token: botToken, chatId: chatId, restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Warning)
                .CreateLogger();
            
            Log.Warning("Starting web host");

            // Add services to the container.
            builder.Host.UseSerilog();

            builder.Services.AddControllers();

            builder.Services.AddCors();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            
            builder.Services.AddLogging();

            builder.Services.AddScoped<DapperDbContext>();
            builder.Services.AddScoped<IDbConnection>(provider =>
                provider.GetService<DapperDbContext>()!.CreateConnection());

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddHttpClient();
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidAudience = builder.Configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
                        ClockSkew = TimeSpan.Zero
                    };
                });

            builder.Services.AddHttpContextAccessor();

            builder.Services.Configure<FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = int.MaxValue;
            });
            builder.WebHost.UseKestrel(options =>
            {
                options.Limits.MaxRequestBodySize = int.MaxValue;
            });
            builder.Services.Configure<KestrelServerOptions>(options =>
            {
                options.Limits.MaxRequestBodySize = int.MaxValue;
            });
            builder.Services.AddScoped<IServiceRepository, ServiceRepository>();
            builder.Services.AddScoped<IApplicationRepository, ApplicationRepository>();
            builder.Services.AddScoped<IApplicationObjectRepository, ApplicationObjectRepository>();
            builder.Services.AddScoped<IArchObjectRepository, ArchObjectRepository>();
            
            builder.Services.AddScoped<IServiceUseCases, ServiceUseCases>();
            builder.Services.AddScoped<IApplicationUseCases, ApplicationUseCases>();
            builder.Services.AddScoped<IApplicationObjectUseCases, ApplicationObjectUseCases>();
            builder.Services.AddScoped<IArchObjectUseCases, ArchObjectUseCases>();

            var app = builder.Build();

            app.UseStaticFiles();
            app.UseMiddlewareExtensions();
            
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true)
                .AllowCredentials());

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();
            
            app.MapControllers();

            app.Run();
        }
    }
}