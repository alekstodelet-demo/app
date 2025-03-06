using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthLibrary
{
    public class AuthService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;

        public AuthService(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

            var serviceProvider = CreateServiceProvider();
            _userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            _signInManager = serviceProvider.GetRequiredService<SignInManager<IdentityUser>>();
            _httpContextAccessor = httpContextAccessor;
        }

        private ServiceProvider CreateServiceProvider()
        {
            var services = new ServiceCollection();

            services.AddLogging();

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(_configuration.GetConnectionString("AuthConnection")));

            services.AddIdentity<IdentityUser, IdentityRole>(options =>
                {
                    options.Password.RequiredLength = 1;
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            return services.BuildServiceProvider();
        }

        public async Task<string?> Login(string email, string password)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    return null;
                }

                if (user != null && await _userManager.CheckPasswordAsync(user, password))
                {
                    var token = GenerateJwtToken(user);
                    return token;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public async Task<UserInfo> GetUserInfo()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext?.User == null || !httpContext.User.Identity.IsAuthenticated)
            {
                return null;
            }

            var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return null;
            }

            var user = await _userManager.FindByEmailAsync(userId);
            if (user == null)
            {
                return null;
            }

            var userInfo = new UserInfo
            {
                Email = user.Email,
                UserName = user.UserName,
                Id = user.Id
            };

            return userInfo;
        }

        public async Task<UserInfo> ChangePassword(string currentPassword, string newPassword)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext?.User == null || !httpContext.User.Identity.IsAuthenticated)
            {
                return null;
            }

            var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return null;
            }

            var user = await _userManager.FindByEmailAsync(userId);

            var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);

            if (result.Succeeded)
            {
                return new UserInfo
                {
                    Email = user.Email,
                    UserName = user.UserName,
                    Id = user.Id
                }; ;
            }
            return null;
        }

        public async Task<string> Register(string email, string password)
        {
            try
            {
                var createUser = new IdentityUser { UserName = email, Email = email };
                var result = await _userManager.CreateAsync(createUser, password);

                if (result.Succeeded)
                {
                    var user = await _userManager.FindByEmailAsync(email);
                    if (user != null)
                    {
                        return user.Id;
                    }
                } else
                {
                    throw new Exception(result.Errors.First().Description);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("User registration failed.", ex);
            }
        }

        public async Task<UserInfo> GetUserByEmail(string userId)
        {
            var user = await _userManager.FindByEmailAsync(userId);
            if (user == null)
            {
                return null;
            }

            var userInfo = new UserInfo
            {
                Email = user.Email,
                UserName = user.UserName,
                Id = user.Id
            };

            return userInfo;
        }

        public class UserInfo
        {
            public string? Email { get; set; }
            public string? UserName { get; set; }
            public string? Id { get; set; }
        }

        public string GenerateJwtToken(IdentityUser user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiryMinutes = _configuration.GetValue<int>("Jwt:ExpiryMinutes");
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(expiryMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
    
    public class NoComplexityPasswordValidator : PasswordValidator<IdentityUser>
    {
        public override async Task<IdentityResult> ValidateAsync(UserManager<IdentityUser> manager, IdentityUser user,   
            string password)
        {
            return IdentityResult.Success;
        }
    }
}