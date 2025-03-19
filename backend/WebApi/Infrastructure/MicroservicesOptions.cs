using Application.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace WebApi.Infrastructure
{
    /// <summary>
    /// Опции для настройки микросервисной архитектуры
    /// </summary>
    public class MicroservicesOptions
    {
        /// <summary>
        /// Флаг включения микросервисной архитектуры
        /// </summary>
        public bool Enabled { get; set; } = false;
        
        /// <summary>
        /// URL сервиса Services
        /// </summary>
        public string ServicesUrl { get; set; } = "http://services-service:8080";
    }

    /// <summary>
    /// Фабрика для создания репозиториев в зависимости от конфигурации
    /// </summary>
    public interface IRepositoryFactory
    {
        /// <summary>
        /// Создает репозиторий услуг
        /// </summary>
        /// <returns>Репозиторий услуг</returns>
        IServiceRepository CreateServiceRepository();
    }

    /// <summary>
    /// Реализация фабрики репозиториев
    /// </summary>
    public class RepositoryFactory : IRepositoryFactory
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly MicroservicesOptions _options;
        
        public RepositoryFactory(
            IServiceProvider serviceProvider,
            IOptions<MicroservicesOptions> options)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        }
        
        /// <summary>
        /// Создает репозиторий услуг в зависимости от конфигурации
        /// </summary>
        public IServiceRepository CreateServiceRepository()
        {
            // Если микросервисы включены, используем микросервисный репозиторий
            if (_options.Enabled)
            {
                return _serviceProvider.GetRequiredService<Repositories.MicroserviceServiceRepository>();
            }
            
            // Иначе используем обычный репозиторий с прямым доступом к БД
            return _serviceProvider.GetRequiredService<Infrastructure.Repositories.ServiceRepository>();
        }
    }
}