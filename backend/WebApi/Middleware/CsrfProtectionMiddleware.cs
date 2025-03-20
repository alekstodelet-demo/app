using Microsoft.AspNetCore.Antiforgery;
using System.Net;

namespace WebApi.Middleware
{
    public class CsrfProtectionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IAntiforgery _antiforgery;
        private readonly ILogger<CsrfProtectionMiddleware> _logger;
        private readonly IConfiguration _configuration;

        public CsrfProtectionMiddleware(
            RequestDelegate next,
            IAntiforgery antiforgery,
            ILogger<CsrfProtectionMiddleware> logger,
            IConfiguration configuration)
        {
            _next = next;
            _antiforgery = antiforgery;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Middleware для автоматической генерации CSRF токена для всех GET запросов
            if (HttpMethods.IsGet(context.Request.Method))
            {
                // Generate token for views
                var tokens = _antiforgery.GetAndStoreTokens(context);
                context.Response.Cookies.Append("XSRF-TOKEN", tokens.RequestToken, new CookieOptions
                {
                    HttpOnly = false, // Client-side JavaScript will need to read this
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    IsEssential = true
                });
            }
            // Проверка CSRF-токена в mutation запросах (POST, PUT, DELETE, PATCH)
            //else if (IsStateChangingMethod(context.Request.Method) && !IsExcludedPath(context.Request.Path))
            //{
            //    try
            //    {
            //        await _antiforgery.ValidateRequestAsync(context);
            //    }
            //    catch (AntiforgeryValidationException ex)
            //    {
            //        _logger.LogWarning(ex, "CSRF validation failed for {Method} {Path}", 
            //            context.Request.Method, context.Request.Path);
                        
            //        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            //        await context.Response.WriteAsJsonAsync(new { 
            //            Error = "CSRF validation failed", 
            //            Message = "Invalid anti-forgery token"
            //        });
                    
            //        return;
            //    }
            //}

            await _next(context);
        }

        private bool IsStateChangingMethod(string method)
        {
            return HttpMethods.IsPost(method) || 
                   HttpMethods.IsPut(method) || 
                   HttpMethods.IsDelete(method) || 
                   HttpMethods.IsPatch(method);
        }

        private bool IsExcludedPath(PathString path)
        {
            // Исключаем API пути, которые используются внешними системами
            // или те, которые аутентифицируются через JWT
            string[] excludedPaths = new[] {
                "/api/v1/Auth/login",
                "/api/v2/auth",
                "/swagger"
            };

            return excludedPaths.Any(p => path.StartsWithSegments(p));
        }
    }

    // Extension method для добавления middleware
    public static class CsrfProtectionMiddlewareExtensions
    {
        public static IApplicationBuilder UseCsrfProtection(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CsrfProtectionMiddleware>();
        }
    }
}