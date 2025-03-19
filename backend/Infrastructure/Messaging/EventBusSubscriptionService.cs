using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Messaging
{
    /// <summary>
    /// Фоновая служба для настройки подписок на события при запуске приложения
    /// </summary>
    public class EventBusSubscriptionService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<EventBusSubscriptionService> _logger;

        public EventBusSubscriptionService(
            IServiceProvider serviceProvider,
            ILogger<EventBusSubscriptionService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await ConfigureSubscriptions();
        }

        /// <summary>
        /// Настройка подписок на события
        /// </summary>
        private async Task ConfigureSubscriptions()
        {
            _logger.LogInformation("Configuring RabbitMQ event bus subscriptions");

            try
            {
                using var scope = _serviceProvider.CreateScope();
                var eventBus = scope.ServiceProvider.GetRequiredService<IEventBus>();

                // Настраиваем подписки на события
                eventBus.Subscribe<ServiceCreatedEvent, ServiceCreatedEventHandler>();
                eventBus.Subscribe<ServiceUpdatedEvent, ServiceUpdatedEventHandler>();
                eventBus.Subscribe<ApplicationCreatedEvent, ApplicationCreatedEventHandler>();

                // Здесь можно добавить больше подписок по мере необходимости

                _logger.LogInformation("Successfully configured event bus subscriptions");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error configuring event bus subscriptions");
                
                // Пробуем повторно через некоторое время
                await Task.Delay(TimeSpan.FromSeconds(10));
                await ConfigureSubscriptions();
            }
        }
    }
}