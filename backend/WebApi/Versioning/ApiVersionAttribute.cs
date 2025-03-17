using Microsoft.AspNetCore.Mvc;

namespace WebApi.Versioning
{
    /// <summary>
    /// Атрибут для указания версии API контроллера
    /// </summary>
    public class ApiVersionAttribute : RouteAttribute
    {
        /// <summary>
        /// Создает атрибут версии API, который автоматически префиксирует маршрут с "api/v{version}"
        /// </summary>
        /// <param name="version">Версия API (например, 1, 2)</param>
        public ApiVersionAttribute(int version)
            : base($"api/v{version}/[controller]")
        {
            Version = version;
        }

        /// <summary>
        /// Получает версию API, указанную в атрибуте
        /// </summary>
        public int Version { get; }
    }
}