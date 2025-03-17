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
using Application.Services;
using Asp.Versioning.ApiExplorer;
using Infrastructure.Security;
using Infrastructure.Services;
using WebApi.Middleware;
using Microsoft.AspNetCore.Mvc;
using WebApi.Configuration;

namespace WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            
            LoggingConfiguration.ConfigureLogging(builder);
            
            var botToken = builder.Configuration["Logging:Token"];
            var chatId = builder.Configuration["Logging:Channel"];
            
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Telegram(token: botToken, chatId: chatId, restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Warning)
                .CreateLogger();
            
            Log.Warning("Starting web host");

            // Add services to the container.
            builder.Host.UseSerilog();
            
            builder.Services.ConfigureApiVersioning();

            // ��������� ������������ � ������� �� XSS
            builder.Services.AddControllers(options =>
            {
                // ��������� ������ �� �������� �������� (CSRF/XSRF)
                //options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            });

            builder.Services.AddControllers();

            builder.Services.AddCors();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.ConfigureSwagger();
            
            builder.Services.AddLogging();

            // ��������� ������ ��� ������������� ��������������
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(20);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.Cookie.SameSite = SameSiteMode.Strict;
            });

            // ����������� �������� ������������
            builder.Services.AddSingleton<IEncryptionService, EncryptionService>();
            builder.Services.Configure<TwoFactorOptions>(builder.Configuration.GetSection("Security:TwoFactor"));
            builder.Services.AddScoped<ITwoFactorService, TwoFactorService>();

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

                    options.RequireHttpsMetadata = true;
                });

            builder.Services.AddHttpContextAccessor();

            builder.Services.Configure<FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = 50 * 1024 * 1024; // 50 MB
            });

            builder.WebHost.UseKestrel(options =>
            {
                options.Limits.MaxRequestBodySize = 50 * 1024 * 1024; // 50 MB
            
                // ��������� HTTPS
                options.ListenAnyIP(5001, listenOptions =>
                {
                    listenOptions.UseHttps("cert.pfx", "secure-password");
                });
            });

            builder.Services.Configure<KestrelServerOptions>(options =>
            {
                options.Limits.MaxRequestBodySize = int.MaxValue;
            });
            
            builder.Services.AddSingleton<IEncryptionService, EncryptionService>();
            builder.Services.AddDapperEncryption();
            
            builder.Services.AddScoped<IServiceRepository, ServiceRepository>();
            builder.Services.AddScoped<IApplicationRepository, ApplicationRepository>();
            builder.Services.AddScoped<IApplicationObjectRepository, ApplicationObjectRepository>();
            builder.Services.AddScoped<IArchObjectRepository, ArchObjectRepository>();
            builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<ITokenService, TokenService>();
            
            builder.Services.AddScoped<IServiceUseCases, ServiceUseCases>();
            builder.Services.AddScoped<IApplicationUseCases, ApplicationUseCases>();
            builder.Services.AddScoped<IApplicationObjectUseCases, ApplicationObjectUseCases>();
            builder.Services.AddScoped<IArchObjectUseCases, ArchObjectUseCases>();

            var app = builder.Build();
            
            EncryptedStringTypeHandler.Initialize(app.Services.GetRequiredService<IEncryptionService>());
            app.UseStaticFiles();
            app.UseMiddlewareExtensions();

            app.Use(async (context, next) =>
            {
                // �������������� ������������
                context.Response.Headers.Append("X-Frame-Options", "DENY");

                // ��������� XSS-������� � ���������
                context.Response.Headers.Append("X-XSS-Protection", "1; mode=block");

                // ������ MIME-���������
                context.Response.Headers.Append("X-Content-Type-Options", "nosniff");

                // Content Security Policy
                context.Response.Headers.Append(
                    "Content-Security-Policy",
                    "default-src 'self'; script-src 'self'; object-src 'none'; img-src 'self'; media-src 'self'; frame-src 'none'; font-src 'self'; connect-src 'self'");

                // Referrer Policy
                context.Response.Headers.Append("Referrer-Policy", "no-referrer-when-downgrade");

                await next();
            });

            if (app.Environment.IsDevelopment())
            {
                app.UseVersionedSwagger(app.Services.GetRequiredService<IApiVersionDescriptionProvider>());
            }
            else
            {
                // ��������� HSTS ������ ��� production
                app.UseHsts();
            }

            app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true)
                .AllowCredentials());

            app.UseHttpsRedirection();

            // ���������� ������ ����� ���������������
            app.UseSession();

            app.UseAuthentication();
            app.UseAuthorization();
            
            app.MapControllers();

            app.Run();
        }
    }
}