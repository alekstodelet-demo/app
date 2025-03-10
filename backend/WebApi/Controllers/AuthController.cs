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

        [HttpPost]
        public IActionResult Authenticate([FromBody] AuthRequest request)
        {
            if (!IsValidRequest(request))
            {
                return BadRequest(new { Message = "Некорректные данные" });
            }
            
            if (IsValidToken(request))
            {
                var tokenString = GenerateJwtToken(request.TokenId);
                return Ok(new { Message = "Авторизация успешна", Token = tokenString });
            }

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
            var key = Encoding.ASCII.GetBytes(_secretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, tokenId),
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }


}
