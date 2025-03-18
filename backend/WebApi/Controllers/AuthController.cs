using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Services;
using Asp.Versioning;
using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WebApi.Dtos;

namespace WebApi.Controllers
{
    [ApiVersion("1.0")]
    [IgnoreAntiforgeryToken]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthController> _logger;
        private readonly ITokenService _tokenService;

        public AuthController(IConfiguration configuration, ILogger<AuthController> logger, ITokenService tokenService)
        {
            _configuration = configuration;
            _logger = logger;
            _tokenService = tokenService;
        }

        [IgnoreAntiforgeryToken]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid authentication request: {@Request}", request);
                return BadRequest(new { Message = "Некорректные данные" });
            }
            
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            var userAgent = Request.Headers["User-Agent"].ToString();
            var deviceId = request.DeviceId ?? Guid.NewGuid().ToString(); 
            
            var authResult = await ValidateTokenAsync(request);
            if (authResult.IsFailed)
            {
                _logger.LogWarning("Authentication failed for TokenId: {TokenId}", request.TokenId);
                return Unauthorized(new { Message = "Invalid PIN or token data" });
            }
            
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, request.TokenId),
                new Claim("ip_address", ipAddress),
                new Claim("user_agent", userAgent.Substring(0, Math.Min(userAgent.Length, 255))),
                new Claim("device_id", deviceId)
            };
            
            var accessToken = _tokenService.GenerateAccessToken(claims);
            var refreshToken = await _tokenService.GenerateRefreshToken(request.TokenId, ipAddress, userAgent, deviceId);
            
            SetRefreshTokenCookie(refreshToken.Token);
            
            _logger.LogInformation("Authentication successful for TokenId: {TokenId}", request.TokenId);
            
            return Ok(new AuthResponse
            {
                AccessToken = accessToken,
                ExpiresIn = 900,
                TokenType = "Bearer",
                RefreshToken = refreshToken.Token,
                DeviceId = deviceId
            });
        }
        
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken()
        {
            // Пытаемся получить refresh token из cookie
            var refreshToken = Request.Cookies["refreshToken"];
            if (string.IsNullOrEmpty(refreshToken))
            {
                return BadRequest(new { Message = "Refresh token is required" });
            }

            // Получаем информацию о запросе
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            var userAgent = Request.Headers["User-Agent"].ToString();
            var deviceId = Request.Headers["X-Device-Id"].ToString() ?? "unknown";

            var result = await _tokenService.RefreshTokens(refreshToken, ipAddress, userAgent, deviceId);
            if (result.IsFailed)
            {
                // Если обновление не удалось, очищаем куки
                Response.Cookies.Delete("refreshToken");
                
                var errorCode = result.Errors[0].Metadata.TryGetValue("ErrorCode", out var code) 
                    ? code?.ToString() 
                    : "REFRESH_TOKEN_ERROR";
                
                if (errorCode == "TOKEN_THEFT_DETECTED")
                {
                    _logger.LogWarning("Token theft detected for user with device ID {DeviceId}", deviceId);
                    return Unauthorized(new { Message = "Security alert: Unusual activity detected. Please login again." });
                }
                
                return BadRequest(new { Message = result.Errors[0].Message });
            }

            var (accessToken, newRefreshToken) = result.Value;

            // Обновляем cookie с refresh токеном
            SetRefreshTokenCookie(newRefreshToken.Token);

            return Ok(new AuthResponse
            {
                AccessToken = accessToken,
                ExpiresIn = 900, // 15 минут в секундах 
                TokenType = "Bearer",
                RefreshToken = newRefreshToken.Token, // Опционально
                DeviceId = deviceId
            });
        }
        
        [Authorize]
        [HttpPost("revoke-token")]
        public async Task<IActionResult> RevokeToken([FromBody] RevokeTokenRequest request)
        {
            // По умолчанию отзываем токен из cookie, если не указан другой токен
            var token = request.RefreshToken ?? Request.Cookies["refreshToken"];
            
            if (string.IsNullOrEmpty(token))
            {
                return BadRequest(new { Message = "Token is required" });
            }

            // Извлекаем идентификатор пользователя из JWT
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Получаем refresh токен из базы
            var refreshTokenResult = await _tokenService.GetRefreshToken(token);
            if (refreshTokenResult.IsFailed)
            {
                return NotFound(new { Message = "Token not found" });
            }

            var refreshToken = refreshTokenResult.Value;

            // Проверяем, принадлежит ли токен текущему пользователю
            if (refreshToken.UserId != userId)
            {
                return Forbid();
            }

            // Отзываем токен
            var result = await _tokenService.RevokeRefreshToken(refreshToken, "Revoked by user");
            
            if (result.IsFailed)
            {
                return BadRequest(new { Message = "Failed to revoke token" });
            }

            // Удаляем cookie
            Response.Cookies.Delete("refreshToken");

            return Ok(new { Message = "Token revoked" });
        }
        
        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest(new { Message = "User not authenticated" });
            }

            // Отзываем все токены пользователя
            await _tokenService.RevokeUserTokens(userId, "User logout");

            // Удаляем cookie
            Response.Cookies.Delete("refreshToken");

            return Ok(new { Message = "Logged out successfully" });
        }
        
        [Authorize]
        [HttpGet("validate")]
        public IActionResult ValidateToken()
        {
            // Этот метод будет доступен только с валидным JWT
            return Ok(new { Message = "Token is valid", UserId = User.FindFirstValue(ClaimTypes.NameIdentifier) });
        }

        private async Task<Result<bool>> ValidateTokenAsync(LoginRequest request)
        {
            var isValid = request.Pin == "1234" &&
                          request.TokenId == "SIMULATED_TOKEN_123" &&
                          request.Signature == "SIMULATED_SIGNATURE";
            
            return Result.Ok(isValid);
        }
        
        private void SetRefreshTokenCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(7)
            };
            Response.Cookies.Append("refreshToken", token, cookieOptions);
        }
    }
}
