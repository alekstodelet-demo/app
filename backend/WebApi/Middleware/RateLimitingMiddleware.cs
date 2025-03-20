using Microsoft.Extensions.Caching.Memory;
using System.Net;
using System.Threading.RateLimiting;

namespace WebApi.Middleware
{
    public class RateLimitingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RateLimitingMiddleware> _logger;
        private readonly IMemoryCache _cache;
        private readonly IConfiguration _configuration;

        public RateLimitingMiddleware(
            RequestDelegate next, 
            ILogger<RateLimitingMiddleware> logger,
            IMemoryCache cache,
            IConfiguration configuration)
        {
            _next = next;
            _logger = logger;
            _cache = cache;
            _configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Проверяем, включено ли ограничение скорости запросов
            if (!_configuration.GetValue<bool>("Security:RateLimit:Enabled", false))
            {
                await _next(context);
                return;
            }

            // Получаем лимиты из конфигурации
            var maxRequestsPerMinute = _configuration.GetValue<int>("Security:RateLimit:MaxRequestsPerMinute", 1000);
            var maxLoginAttemptsPerHour = _configuration.GetValue<int>("Security:RateLimit:MaxLoginAttemptsPerHour", 500);

            // Определяем идентификатор клиента (IP + User-Agent + DeviceId при наличии)
            var clientId = GetClientIdentifier(context);
            
            // Особое ограничение для аутентификационных запросов
            if (IsAuthEndpoint(context))
            {
                if (await CheckAuthRateLimit(context, clientId, maxLoginAttemptsPerHour))
                {
                    await _next(context);
                }
                return;
            }

            // Общее ограничение для других запросов
            if (await CheckGeneralRateLimit(context, clientId, maxRequestsPerMinute))
            {
                await _next(context);
            }
        }

        private bool IsAuthEndpoint(HttpContext context)
        {
            var path = context.Request.Path.Value?.ToLower();
            return path != null && (
                path.Contains("/auth/login") || 
                path.Contains("/auth/refresh-token") || 
                path.EndsWith("/token")
            );
        }

        private async Task<bool> CheckAuthRateLimit(HttpContext context, string clientId, int maxAttempts)
        {
            var cacheKey = $"auth_limiter_{clientId}";
            
            // Проверяем текущее количество попыток
            var counter = _cache.GetOrCreate(cacheKey, entry =>
            {
                // Устанавливаем срок действия записи на 1 час
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);
                return new RateLimitCounter { Count = 0, LastReset = DateTime.UtcNow };
            });

            // Увеличиваем счетчик
            Interlocked.Increment(ref counter.Count);
            _cache.Set(cacheKey, counter);

            // Если лимит превышен
            if (counter.Count > maxAttempts)
            {
                var retryAfter = counter.LastReset.AddHours(1) - DateTime.UtcNow;
                
                _logger.LogWarning("Rate limit exceeded for auth requests. Client: {ClientId}, Count: {Count}", 
                    clientId, counter.Count);
                
                context.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
                context.Response.Headers.Append("Retry-After", retryAfter.TotalSeconds.ToString());
                
                await context.Response.WriteAsJsonAsync(new 
                { 
                    Error = "Too many authentication attempts", 
                    RetryAfter = Math.Ceiling(retryAfter.TotalMinutes)
                });
                
                return false;
            }
            
            return true;
        }

        private async Task<bool> CheckGeneralRateLimit(HttpContext context, string clientId, int maxRequests)
        {
            var cacheKey = $"general_limiter_{clientId}";
            
            // Проверяем текущее количество запросов
            var counter = _cache.GetOrCreate(cacheKey, entry =>
            {
                // Устанавливаем срок действия записи на 1 минуту
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1);
                return new RateLimitCounter { Count = 0, LastReset = DateTime.UtcNow };
            });

            // Увеличиваем счетчик
            Interlocked.Increment(ref counter.Count);
            _cache.Set(cacheKey, counter);

            // Если лимит превышен
            if (counter.Count > maxRequests)
            {
                var retryAfter = counter.LastReset.AddMinutes(1) - DateTime.UtcNow;
                
                _logger.LogWarning("Rate limit exceeded for general requests. Client: {ClientId}, Count: {Count}", 
                    clientId, counter.Count);
                
                context.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
                context.Response.Headers.Append("Retry-After", retryAfter.TotalSeconds.ToString());
                
                await context.Response.WriteAsJsonAsync(new 
                { 
                    Error = "Rate limit exceeded", 
                    RetryAfter = Math.Ceiling(retryAfter.TotalSeconds)
                });
                
                return false;
            }
            
            return true;
        }

        private string GetClientIdentifier(HttpContext context)
        {
            var ip = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            var userAgent = context.Request.Headers["User-Agent"].ToString();
            var deviceId = context.Request.Headers["X-Device-Id"].ToString();

            // Создаем хеш идентификатора пользователя, чтобы избежать хранения потенциально конфиденциальной информации
            using var sha = System.Security.Cryptography.SHA256.Create();
            var combined = $"{ip}|{userAgent}|{deviceId}";
            var hash = Convert.ToBase64String(sha.ComputeHash(System.Text.Encoding.UTF8.GetBytes(combined)));
            
            return hash;
        }
    }

    internal class RateLimitCounter
    {
        public int Count;
        public DateTime LastReset;
    }
    
    // Extension method для добавления middleware
    public static class RateLimitingMiddlewareExtensions
    {
        public static IApplicationBuilder UseRateLimiting(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RateLimitingMiddleware>();
        }
    }
}