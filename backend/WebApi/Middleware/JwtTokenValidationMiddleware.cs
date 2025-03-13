using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace WebApi.Middleware
{
    /// <summary>
    /// Middleware для расширенной валидации JWT токенов
    /// Проверяет не только стандартные параметры (подпись, срок действия),
    /// но и дополнительные факторы безопасности (IP, user agent)
    /// </summary>
    public class JwtTokenValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<JwtTokenValidationMiddleware> _logger;

        public JwtTokenValidationMiddleware(RequestDelegate next, ILogger<JwtTokenValidationMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context, ITokenService tokenService)
        {
            // Проверяем, есть ли токен авторизации в запросе
            var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
            if (authHeader != null && authHeader.StartsWith("Bearer "))
            {
                var token = authHeader.Substring("Bearer ".Length).Trim();
                var validationResult = await ValidateToken(context, token, tokenService);

                if (!validationResult)
                {
                    // Если токен недействителен, отклоняем запрос
                    context.Response.StatusCode = 401; // Unauthorized
                    await context.Response.WriteAsJsonAsync(new { Message = "Invalid or revoked token" });
                    return;
                }
            }

            await _next(context);
        }

        private async Task<bool> ValidateToken(HttpContext context, string token, ITokenService tokenService)
        {
            try
            {
                // Шаг 1: Базовая валидация токена (проверка подписи, срока и т.д.)
                var validationResult = tokenService.ValidateAccessToken(token);
                if (validationResult.IsFailed)
                {
                    _logger.LogWarning("Token validation failed: {Error}", validationResult.Errors.First().Message);
                    return false;
                }

                var principal = validationResult.Value;

                // Шаг 2: Проверяем, не был ли токен отозван
                var jti = principal.FindFirstValue(JwtRegisteredClaimNames.Jti);
                if (string.IsNullOrEmpty(jti) || await tokenService.IsTokenRevoked(jti))
                {
                    _logger.LogWarning("Token has been revoked: {Jti}", jti);
                    return false;
                }

                // Шаг 3: Дополнительные проверки (IP адрес, user agent)
                var ipClaim = principal.FindFirstValue("ip_address");
                var userAgentClaim = principal.FindFirstValue("user_agent");
                var deviceIdClaim = principal.FindFirstValue("device_id");

                var currentIp = context.Connection.RemoteIpAddress?.ToString();
                var currentUserAgent = context.Request.Headers["User-Agent"].ToString();
                var currentDeviceId = context.Request.Headers["X-Device-Id"].ToString();

                // Проверка device ID (строгая)
                if (!string.IsNullOrEmpty(deviceIdClaim) && !string.IsNullOrEmpty(currentDeviceId) && 
                    deviceIdClaim != currentDeviceId)
                {
                    _logger.LogWarning("Device ID mismatch: {Expected} vs {Actual}", deviceIdClaim, currentDeviceId);
                    return false;
                }

                // Проверка User-Agent (не строгая - браузеры могут немного различаться в деталях User-Agent)
                // Некоторые приложения могут решить полностью отключить эту проверку или сделать её очень гибкой
                if (!string.IsNullOrEmpty(userAgentClaim) && !string.IsNullOrEmpty(currentUserAgent) &&
                    !currentUserAgent.Contains(userAgentClaim.Substring(0, Math.Min(30, userAgentClaim.Length))))
                {
                    _logger.LogWarning("User agent mismatch");
                    // Здесь можно решить либо отклонить, либо только логировать
                    // В данном случае просто логируем, но не отклоняем
                }

                // IP адрес может меняться (мобильные сети, VPN и т.д.)
                // Поэтому мы его проверяем, но не отклоняем запрос при несоответствии
                if (!string.IsNullOrEmpty(ipClaim) && !string.IsNullOrEmpty(currentIp) && 
                    ipClaim != currentIp)
                {
                    _logger.LogInformation("IP address changed: {OldIp} -> {NewIp}", ipClaim, currentIp);
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during token validation");
                return false;
            }
        }
    }

    // Extension method для добавления middleware
    public static class JwtTokenValidationMiddlewareExtensions
    {
        public static IApplicationBuilder UseJwtTokenValidation(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<JwtTokenValidationMiddleware>();
        }
    }
}