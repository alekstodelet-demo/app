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
using System.Globalization;
using System.Text;
using Application.Services;
using Asp.Versioning.ApiExplorer;
using Infrastructure.Security;
using Infrastructure.Services;
using Microsoft.AspNetCore.Localization;
using WebApi.Middleware;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WebApi.Configuration;
using WebApi.Filters;
using WebApi.Resources;
using WebApi.Services;
using FluentValidation;
using WebApi.Dtos;
using WebApi.Validations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            
            LoggingConfiguration.ConfigureLogging(builder);
            
            Log.Warning("Starting web host");

            // Add services to the container.
            builder.Host.UseSerilog();
            
            builder.Services.ConfigureApiVersioning();

            // ��������� ������������ � ������� �� XSS
            builder.Services.AddControllers(options =>
            {
                // Добавление глобального фильтра для проверки CSRF токенов
                options.Filters.Add<ValidateAntiforgeryTokenFilter>();
            });

            builder.Services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver
                    {
                        NamingStrategy = new Newtonsoft.Json.Serialization.SnakeCaseNamingStrategy()
                    };
                });


            builder.Services.AddCors();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.ConfigureSwagger();
            
            builder.Services.AddLogging();
            
            // Register the request timing options (optional - if you want to use custom settings)
            builder.Services.Configure<RequestTimingOptions>(options => {
                options.MinDurationToLog = 300; // Log requests that take longer than 300ms
                options.LogAllRequests = true;  // Set to false in production to reduce log volume
                options.ExcludePaths = new[] { "/health", "/metrics", "/favicon.ico", "/static" };
            });

            builder.Services.AddAntiforgery(options =>
            {
                // Настройка генерации токенов для защиты от CSRF
                options.HeaderName = "X-XSRF-TOKEN"; // Имя заголовка, в котором клиент должен передавать токен
                options.Cookie.Name = "XSRF-TOKEN";  // Имя cookie, в котором хранится токен
                options.Cookie.HttpOnly = false;     // JavaScript должен иметь доступ к токену
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Требуем HTTPS
                options.Cookie.SameSite = SameSiteMode.Strict; // Ограничиваем доступ только с того же сайта
                options.SuppressXFrameOptionsHeader = false;   // Добавляем X-Frame-Options header
            });
            
            builder.Services.AddMemoryCache();

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
            
            builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
            
            builder.Services.AddValidation();
            
            builder.Services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new List<CultureInfo>
                {
                    new CultureInfo("en"),
                    new CultureInfo("ru"),
                    new CultureInfo("ky")
                };

                options.DefaultRequestCulture = new RequestCulture("ru");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
    
                // Добавляем провайдеры для определения культуры
                options.RequestCultureProviders = new List<IRequestCultureProvider>
                {
                    new QueryStringRequestCultureProvider(),
                    new CookieRequestCultureProvider(),
                    new AcceptLanguageHeaderRequestCultureProvider()
                };
            });
            
            builder.Services.AddSingleton<IValidationLocalizerService, ValidationLocalizerService>();
            
            builder.Services.AddHttpContextAccessor();
            
            builder.Services.AddLocalizationServices();

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
            
                var certPath = builder.Configuration["Security:Certificates:PfxPath"];
                var certPassword = builder.Configuration["Security:Certificates:Password"];
                
                if (string.IsNullOrEmpty(certPath) || string.IsNullOrEmpty(certPassword))
                {
                    throw new InvalidOperationException("HTTPS certificate path or password is not configured.");
                }

                options.ListenAnyIP(5001, listenOptions =>
                {
                    listenOptions.UseHttps(certPath, certPassword);
                });
            });

            builder.Services.Configure<KestrelServerOptions>(options =>
            {
                options.Limits.MaxRequestBodySize = int.MaxValue;
            });

            builder.Services.AddDapperEncryption();
            
            builder.Services.AddScoped<IServiceRepository, ServiceRepository>();
            builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
            builder.Services.AddScoped<ICustomerContactRepository, CustomerContactRepository>();
            builder.Services.AddScoped<IContactTypeRepository, ContactTypeRepository>();
            builder.Services.AddScoped<IApplicationRepository, ApplicationRepository>();
            builder.Services.AddScoped<IApplicationObjectRepository, ApplicationObjectRepository>();
            builder.Services.AddScoped<IArchObjectRepository, ArchObjectRepository>();
            builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<ITokenService, TokenService>();
            
            builder.Services.AddScoped<IServiceUseCases, ServiceUseCases>();
            builder.Services.AddScoped<ICustomerUseCases, CustomerUseCases>();
            builder.Services.AddScoped<ICustomerContactUseCases, CustomerContactUseCases>();
            builder.Services.AddScoped<IContactTypeUseCases, ContactTypeUseCases>();
            builder.Services.AddScoped<IUserUseCases, UserUseCases>();
            builder.Services.AddScoped<IApplicationUseCases, ApplicationUseCases>();
            builder.Services.AddScoped<IApplicationObjectUseCases, ApplicationObjectUseCases>();
            builder.Services.AddScoped<IArchObjectUseCases, ArchObjectUseCases>();
            builder.Services.AddScoped<IAlertService, AlertService>();
            builder.Services.AddScoped<ILoggingService, LoggingService>();
            // Регистрация сервисов
            builder.Services.AddScoped<IRegistrationRepository, RegistrationRepository>();
            builder.Services.AddScoped<IRegistrationService, RegistrationService>();
            builder.Services.AddScoped<IValidator<RegisterRequest>, RegisterRequestValidator>();

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
                app.UseSwagger(); // Add this line before UseVersionedSwagger
                app.UseVersionedSwagger(app.Services.GetRequiredService<IApiVersionDescriptionProvider>());
            }
            else
            {
                // ��������� HSTS ������ ��� production
                app.UseHsts();
            }
            
            var localizationOptions = app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(localizationOptions.Value);
            
            //app.UseCsrfProtection();
            
            app.UseLocalizationConfiguration();
            
            //app.UseRateLimiting();
            
            // Add the request timing middleware early in the pipeline
            // to capture the total request processing time
            app.UseRequestTiming();

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