using Infrastructure.Messaging.Handlers;
using Messaging.Shared.RabbitMQ;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace Infrastructure.Messaging
{
    public static class RabbitMQConfiguration
    {
        public static IServiceCollection AddRabbitMQMessaging(this IServiceCollection services, IConfiguration configuration)
        {
            // Конфигурация RabbitMQ
            var rabbitConfig = configuration.GetSection("RabbitMQ");
            var host = rabbitConfig["Host"] ?? "localhost";
            var port = int.TryParse(rabbitConfig["Port"], out var portValue) ? portValue : 5672;
            var username = rabbitConfig["Username"] ?? "guest";
            var password = rabbitConfig["Password"] ?? "guest";
            var virtualHost = rabbitConfig["VirtualHost"] ?? "/";

            // Создаем фабрику подключений
            services.AddSingleton<IConnectionFactory>(sp => 
                new ConnectionFactory
                {
                    HostName = host,
                    Port = port,
                    UserName = username,
                    Password = password,
                    VirtualHost = virtualHost,
                    DispatchConsumersAsync = true // Включаем асинхронную обработку
                });

            // Добавляем RabbitMQ connection
            services.AddSingleton<IRabbitMQConnection, RabbitMQConnection>();
            
            // Добавляем сервисы шины событий
            services.AddSingleton<IEventBus, RabbitMQEventBus>();
            
            // Добавляем обработчики событий
            services.AddTransient<ServiceCreatedEventHandler>();
            services.AddTransient<ServiceUpdatedEventHandler>();
            services.AddTransient<ApplicationCreatedEventHandler>();
            
            // Добавляем сервис подписки на события
            services.AddHostedService<EventBusSubscriptionService>();

            return services;
        }
    }
}