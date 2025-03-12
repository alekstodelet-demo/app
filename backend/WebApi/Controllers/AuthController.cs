using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WebApi.Dtos;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly string _secretKey = "your-very-secret-key-32-chars-long";
        
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IConfiguration configuration, ILogger<AuthController> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        [HttpPost]
        public IActionResult Authenticate([FromBody] AuthRequest request)
        {
            if (!IsValidRequest(request))
            {
                _logger.LogWarning("Invalid authentication request: {@Request}", request);
                return BadRequest(new { Message = "Некорректные данные" });
            }
            
            if (IsValidToken(request))
            {
                var tokenString = GenerateJwtToken(request.TokenId);
                _logger.LogInformation("Authentication successful for TokenId: {TokenId}", request.TokenId);
                return Ok(new { Message = "Авторизация успешна", Token = tokenString });
            }
            
            _logger.LogWarning("Authentication failed for TokenId: {TokenId}", request.TokenId);
            return Unauthorized(new { Message = "Неверный PIN или данные токена" });
        }

        private bool IsValidRequest(AuthRequest request)
        {
            return request != null &&
                   !string.IsNullOrEmpty(request.Pin) &&
                   !string.IsNullOrEmpty(request.TokenId) &&
                   !string.IsNullOrEmpty(request.Signature);
        }

        private bool IsValidToken(AuthRequest request)
        {
            return request.Pin == "1234" &&
                   request.TokenId == "SIMULATED_TOKEN_123" &&
                   request.Signature == "SIMULATED_SIGNATURE";
        }

        private string GenerateJwtToken(string tokenId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, tokenId),
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }


}
