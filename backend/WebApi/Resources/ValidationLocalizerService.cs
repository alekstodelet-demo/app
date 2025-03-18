using System.Globalization;
using System.Reflection;
using Microsoft.Extensions.Localization;

namespace WebApi.Resources
{
    /// <summary>
    /// Интерфейс сервиса локализации для валидаций
    /// </summary>
    public interface IValidationLocalizerService
    {
        /// <summary>
        /// Получает локализованную строку по ключу
        /// </summary>
        /// <param name="key">Ключ строки</param>
        /// <returns>Локализованная строка</returns>
        string GetString(string key);

        /// <summary>
        /// Получает локализованную строку по ключу с подстановкой параметров
        /// </summary>
        /// <param name="key">Ключ строки</param>
        /// <param name="args">Параметры для подстановки</param>
        /// <returns>Локализованная строка с подставленными параметрами</returns>
        string GetString(string key, params object[] args);
    }
    
    /// <summary>
    /// Реализация сервиса локализации для валидаций
    /// </summary>
    public class ValidationLocalizerService : IValidationLocalizerService
    {
        private readonly IStringLocalizer _localizer;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ValidationLocalizerService(
            IStringLocalizerFactory localizerFactory,
            IHttpContextAccessor httpContextAccessor)
        {
            _localizer = localizerFactory.Create("ValidationMessages", Assembly.GetExecutingAssembly().FullName);
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Получает текущую культуру из HTTP-запроса или настроек приложения
        /// </summary>
        private CultureInfo GetCurrentCulture()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext != null)
            {
                // Сначала пробуем получить культуру из HTTP-заголовка Accept-Language
                var acceptLanguage = httpContext.Request.Headers["Accept-Language"].ToString();
                if (!string.IsNullOrEmpty(acceptLanguage))
                {
                    try
                    {
                        return new CultureInfo(acceptLanguage.Split(',')[0].Trim());
                    }
                    catch { /* Игнорируем ошибки при создании CultureInfo */ }
                }

                // Затем из cookies, если есть
                if (httpContext.Request.Cookies.TryGetValue("culture", out var cultureCookie))
                {
                    try
                    {
                        return new CultureInfo(cultureCookie);
                    }
                    catch { /* Игнорируем ошибки при создании CultureInfo */ }
                }
            }

            // По умолчанию используем настройки приложения
            return CultureInfo.CurrentUICulture;
        }

        /// <summary>
        /// Получает локализованную строку по ключу
        /// </summary>
        public string GetString(string key)
        {
            // Устанавливаем текущую культуру для текущего запроса
            var currentCulture = GetCurrentCulture();
            var originalCulture = CultureInfo.CurrentUICulture;
            
            try
            {
                CultureInfo.CurrentUICulture = currentCulture;
                return _localizer[key];
            }
            finally
            {
                // Восстанавливаем оригинальную культуру
                CultureInfo.CurrentUICulture = originalCulture;
            }
        }

        /// <summary>
        /// Получает локализованную строку по ключу с подстановкой параметров
        /// </summary>
        public string GetString(string key, params object[] args)
        {
            // Устанавливаем текущую культуру для текущего запроса
            var currentCulture = GetCurrentCulture();
            var originalCulture = CultureInfo.CurrentUICulture;
            
            try
            {
                CultureInfo.CurrentUICulture = currentCulture;
                return _localizer[key, args];
            }
            finally
            {
                // Восстанавливаем оригинальную культуру
                CultureInfo.CurrentUICulture = originalCulture;
            }
        }
    }
}