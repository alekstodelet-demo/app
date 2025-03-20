using System.Data;
using Application.Repositories;
using Infrastructure.Repositories;
using Microsoft.OpenApi.Models;
using Messaging.Shared;
using Messaging.Shared.Events;
using Messaging.Shared.RabbitMQ;
using Messaging.Shared.Services;
using Services.Microservice.EventHandlers;
using Serilog;
using Serilog.Events;

namespace Services.Microservice
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configure Serilog
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .Enrich.WithProperty("Application", "Services.Microservice")
                // .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
                // .WriteTo.File("logs/services-microservice-.log", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            // builder.Host.UseSerilog();

            // Add services to the container
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Services Microservice API", Version = "v1" });
            });

            // Configure RabbitMQ
            builder.Services.Configure<RabbitMQOptions>(builder.Configuration.GetSection("RabbitMQ"));
            builder.Services.AddSingleton<IRabbitMQConnection, RabbitMQConnection>();

            // Configure EventBus
            builder.Services.AddSingleton<IEventBus>(sp =>
            {
                var rabbitMQConnection = sp.GetRequiredService<IRabbitMQConnection>();
                var logger = sp.GetRequiredService<ILogger<RabbitMQEventBus>>();
                var serviceName = "services";
                var queueName = $"bga_{serviceName}_queue"; // Stable queue name for the service type

                return new RabbitMQEventBus(
                    rabbitMQConnection,
                    logger,
                    sp,
                    queueName);
            });

            // Register repositories
            builder.Services.AddScoped<DapperDbContext>();
            builder.Services.AddScoped<IDbConnection>(provider =>
                provider.GetService<DapperDbContext>()!.CreateConnection());
            builder.Services.AddScoped<IServiceRepository, ServiceRepository>();

            // Register event handlers
            builder.Services.AddTransient<ServiceRequestedEventHandler>();
            builder.Services.AddTransient<ServiceCreatedEventHandler>();
            builder.Services.AddTransient<ServiceUpdatedEventHandler>();
            builder.Services.AddTransient<ServiceDeletedEventHandler>();

            // Register event subscription service
            builder.Services.AddHostedService<EventBusSubscriptionService>();

            builder.Services.AddControllers();

            var app = builder.Build();

            // Configure the HTTP request pipeline
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            // Subscribe to events
            var eventBus = app.Services.GetRequiredService<IEventBus>();
            eventBus.Subscribe<ServiceRequestedEvent, ServiceRequestedEventHandler>();
            eventBus.Subscribe<ServiceCreatedEvent, ServiceCreatedEventHandler>();
            eventBus.Subscribe<ServiceUpdatedEvent, ServiceUpdatedEventHandler>();
            eventBus.Subscribe<ServiceDeletedEvent, ServiceDeletedEventHandler>();

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
