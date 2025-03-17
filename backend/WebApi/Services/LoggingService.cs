using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace WebApi.Services
{
    /// <summary>
    /// Service providing advanced structured logging capabilities
    /// </summary>
    public interface ILoggingService
    {
        void LogRequest(string endpoint, string method, int statusCode, long elapsedMilliseconds, string userId = null);
        void LogUserActivity(string userId, string activity, Dictionary<string, object> additionalData = null);
        void LogBusinessEvent(string eventName, Dictionary<string, object> eventData);
        void LogPerformanceMetric(string operation, long elapsedMilliseconds, Dictionary<string, object> metadata = null);
        void LogSecurityEvent(string eventType, string userId, string ipAddress, Dictionary<string, object> eventData = null);
        void LogDatabaseOperation(string operation, string entity, long elapsedMilliseconds, bool success, string errorMessage = null);
        Task<bool> SendAlertAsync(string subject, string message, AlertSeverity severity);
    }

    public enum AlertSeverity
    {
        Info,
        Warning,
        Error,
        Critical
    }

    /// <summary>
    /// Implementation of the logging service
    /// </summary>
    public class LoggingService : ILoggingService
    {
        private readonly ILogger<LoggingService> _logger;
        private readonly IAlertService _alertService;

        public LoggingService(ILogger<LoggingService> logger, IAlertService alertService)
        {
            _logger = logger;
            _alertService = alertService;
        }

        public void LogRequest(string endpoint, string method, int statusCode, long elapsedMilliseconds, string userId = null)
        {
            var logLevel = statusCode >= 500 ? LogLevel.Error :
                          (statusCode >= 400 ? LogLevel.Warning : LogLevel.Information);

            _logger.Log(logLevel, 
                "HTTP {Method} {Endpoint} responded {StatusCode} in {ElapsedMilliseconds}ms {UserId}",
                method, endpoint, statusCode, elapsedMilliseconds, userId ?? "anonymous");
            
            // Send alert for server errors
            if (statusCode >= 500)
            {
                _ = _alertService.SendAlertAsync(
                    "Server Error",
                    $"Endpoint {method} {endpoint} returned {statusCode}",
                    AlertSeverity.Error
                );
            }
        }

        public void LogUserActivity(string userId, string activity, Dictionary<string, object> additionalData = null)
        {
            additionalData ??= new Dictionary<string, object>();
            additionalData["UserId"] = userId;
            additionalData["Activity"] = activity;
            additionalData["Timestamp"] = DateTime.UtcNow;

            _logger.LogInformation("User activity: {Activity} by {UserId} {@AdditionalData}", 
                activity, userId, additionalData);
        }

        public void LogBusinessEvent(string eventName, Dictionary<string, object> eventData)
        {
            eventData ??= new Dictionary<string, object>();
            eventData["EventName"] = eventName;
            eventData["Timestamp"] = DateTime.UtcNow;

            _logger.LogInformation("Business event: {EventName} {@EventData}", eventName, eventData);
        }

        public void LogPerformanceMetric(string operation, long elapsedMilliseconds, Dictionary<string, object> metadata = null)
        {
            var logLevel = elapsedMilliseconds > 1000 ? LogLevel.Warning : LogLevel.Debug;
            
            _logger.Log(logLevel, 
                "Performance: {Operation} took {ElapsedMilliseconds}ms {@Metadata}", 
                operation, elapsedMilliseconds, metadata ?? new Dictionary<string, object>());
            
            // Send alert for very slow operations
            if (elapsedMilliseconds > 5000) // 5 seconds
            {
                _ = _alertService.SendAlertAsync(
                    "Performance Alert",
                    $"Operation {operation} took {elapsedMilliseconds}ms to complete",
                    AlertSeverity.Warning
                );
            }
        }

        public void LogSecurityEvent(string eventType, string userId, string ipAddress, Dictionary<string, object> eventData = null)
        {
            eventData ??= new Dictionary<string, object>();
            eventData["UserId"] = userId;
            eventData["IpAddress"] = ipAddress;
            eventData["EventType"] = eventType;
            eventData["Timestamp"] = DateTime.UtcNow;

            _logger.LogWarning("Security event: {EventType} for {UserId} from {IpAddress} {@EventData}", 
                eventType, userId, ipAddress, eventData);
            
            // Always alert on security events
            _ = _alertService.SendAlertAsync(
                "Security Event",
                $"{eventType} detected for user {userId} from IP {ipAddress}",
                AlertSeverity.Warning
            );
        }

        public void LogDatabaseOperation(string operation, string entity, long elapsedMilliseconds, bool success, string errorMessage = null)
        {
            var level = success ? LogLevel.Debug : LogLevel.Error;
            
            if (success)
            {
                _logger.Log(level, 
                    "Database: {Operation} on {Entity} completed in {ElapsedMilliseconds}ms", 
                    operation, entity, elapsedMilliseconds);
            }
            else
            {
                _logger.Log(level, 
                    "Database: {Operation} on {Entity} failed after {ElapsedMilliseconds}ms with error: {ErrorMessage}", 
                    operation, entity, elapsedMilliseconds, errorMessage);
                
                // Send alert for database errors
                _ = _alertService.SendAlertAsync(
                    "Database Error",
                    $"Operation {operation} on {entity} failed: {errorMessage}",
                    AlertSeverity.Error
                );
            }
        }

        public async Task<bool> SendAlertAsync(string subject, string message, AlertSeverity severity)
        {
            return await _alertService.SendAlertAsync(subject, message, severity);
        }
    }
}