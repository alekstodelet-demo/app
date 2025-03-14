using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;
using Serilog.Sinks.Grafana.Loki;

namespace WebApi.Configuration
{
    public static class LoggingConfiguration
    {
        public static void ConfigureLogging(WebApplicationBuilder builder)
        {
            var lokiUrl = builder.Configuration["Logging:Loki:Url"];
            
            // Create Serilog logger configuration
            var loggerConfig = new LoggerConfiguration()
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .Enrich.WithProperty("Application", "BGA.WebApi");

            // Always add Console sink for development and troubleshooting
            loggerConfig = loggerConfig.WriteTo.Console(
                outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}");

            // Add File sink for persistent logs
            loggerConfig = loggerConfig.WriteTo.File(
                new JsonFormatter(), 
                "logs/bgawebapi-.log", 
                rollingInterval: RollingInterval.Day,
                retainedFileCountLimit: 31);

            // Add Grafana Loki sink if configured
            if (!string.IsNullOrEmpty(lokiUrl))
            {
                loggerConfig = loggerConfig.WriteTo.GrafanaLoki(
                    lokiUrl,
                    labels: new[] { 
                        new LokiLabel { Key = "app", Value = "bga-webapi" },
                        new LokiLabel { Key = "env", Value = builder.Environment.EnvironmentName }
                    },
                    restrictedToMinimumLevel: LogEventLevel.Information);
            }
            
            // Create and set the logger
            Log.Logger = loggerConfig.CreateLogger();
            
            // Use Serilog for logging
            builder.Host.UseSerilog();
        }
    }
}