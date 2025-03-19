using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApi.Filters
{
    /// <summary>
    /// Фильтр для глобальной проверки CSRF токенов для опасных HTTP методов
    /// </summary>
    public class ValidateAntiforgeryTokenFilter : IAsyncAuthorizationFilter
    {
        private readonly IAntiforgery _antiforgery;
        private readonly ILogger<ValidateAntiforgeryTokenFilter> _logger;

        public ValidateAntiforgeryTokenFilter(IAntiforgery antiforgery, ILogger<ValidateAntiforgeryTokenFilter> logger)
        {
            _antiforgery = antiforgery;
            _logger = logger;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            //// Проверяем, что этот метод требует проверки CSRF токена
            //if (ShouldValidate(context))
            //{
            //    try
            //    {
            //        await _antiforgery.ValidateRequestAsync(context.HttpContext);
            //    }
            //    catch (AntiforgeryValidationException ex)
            //    {
            //        _logger.LogWarning(ex, "CSRF validation failed for {Method} {Path}",
            //            context.HttpContext.Request.Method, context.HttpContext.Request.Path);

            //        context.Result = new BadRequestObjectResult(new
            //        {
            //            Error = "CSRF validation failed",
            //            Message = "Invalid anti-forgery token"
            //        });
            //    }
            //}
        }

        private bool ShouldValidate(AuthorizationFilterContext context)
        {
            // Проверяем только мутирующие запросы, которые не помечены атрибутами пропуска
            var method = context.HttpContext.Request.Method;
            
            // Пропускаем GET, HEAD, OPTIONS, TRACE
            if (HttpMethods.IsGet(method) || 
                HttpMethods.IsHead(method) || 
                HttpMethods.IsOptions(method) || 
                HttpMethods.IsTrace(method))
            {
                return false;
            }

            // Пропускаем запросы со специальными атрибутами
            if (context.Filters.Any(filter => filter is IDisableAntiforgeryCheck))
            {
                return false;
            }

            // Пропускаем API запросы, которые используют Bearer токены
            var path = context.HttpContext.Request.Path.Value?.ToLower() ?? "";
            if (path.StartsWith("/api/") && HasAuthorizationHeader(context.HttpContext))
            {
                return false;
            }

            return true;
        }

        private bool HasAuthorizationHeader(HttpContext context)
        {
            return context.Request.Headers.ContainsKey("Authorization") &&
                   context.Request.Headers["Authorization"].ToString().StartsWith("Bearer ");
        }
    }

    /// <summary>
    /// Интерфейс-маркер для отключения проверки CSRF
    /// </summary>
    public interface IDisableAntiforgeryCheck { }

    /// <summary>
    /// Атрибут для отключения проверки CSRF для отдельных контроллеров или методов
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class IgnoreAntiforgeryTokenAttribute : Attribute, IDisableAntiforgeryCheck
    {
    }
}