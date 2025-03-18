using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace WebApi.Middleware
{
    /// <summary>
    /// Configuration options for the request timing middleware
    /// </summary>
    public class RequestTimingOptions
    {
        /// <summary>
        /// Minimum duration in milliseconds to log. Requests faster than this won't be logged.
        /// </summary>
        public int MinDurationToLog { get; set; } = 500;

        /// <summary>
        /// Whether to log all requests or only slow ones
        /// </summary>
        public bool LogAllRequests { get; set; } = false;

        /// <summary>
        /// Whether to include request body in logs (warning: may contain sensitive data)
        /// </summary>
        public bool IncludeRequestBody { get; set; } = false;

        /// <summary>
        /// Whether to include response body in logs (warning: may contain sensitive data)
        /// </summary>
        public bool IncludeResponseBody { get; set; } = false;

        /// <summary>
        /// List of paths to exclude from timing (e.g., health checks, static files)
        /// </summary>
        public string[] ExcludePaths { get; set; } = new string[] { "/health", "/favicon.ico" };
    }

    /// <summary>
    /// Middleware for logging API request timing information
    /// </summary>
    public class RequestTimingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestTimingMiddleware> _logger;
        private readonly RequestTimingOptions _options;

        public RequestTimingMiddleware(
            RequestDelegate next,
            ILogger<RequestTimingMiddleware> logger,
            IOptions<RequestTimingOptions> options)
        {
            _next = next;
            _logger = logger;
            _options = options.Value;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Check if this path should be excluded
            string path = context.Request.Path.ToString().ToLower();
            if (ShouldExcludePath(path))
            {
                await _next(context);
                return;
            }

            // Start the stopwatch
            var stopwatch = Stopwatch.StartNew();
            var requestMethod = context.Request.Method;
            var requestPath = context.Request.Path;
            var requestQuery = context.Request.QueryString;

            // Store the original body stream
            var originalBodyStream = context.Response.Body;
            string responseBody = string.Empty;

            try
            {
                // Continue down the middleware pipeline, measuring time
                await _next(context);
            }
            finally
            {
                // Stop the stopwatch
                stopwatch.Stop();
                long elapsedMilliseconds = stopwatch.ElapsedMilliseconds;

                // Log if duration exceeds threshold or if logAll is true
                if (_options.LogAllRequests || elapsedMilliseconds >= _options.MinDurationToLog)
                {
                    // Gather information to log
                    var statusCode = context.Response.StatusCode;
                    var userInfo = context.User?.Identity?.Name ?? "anonymous";
                    var userIp = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";

                    // Determine log level based on response time and status code
                    LogLevel logLevel = DetermineLogLevel(elapsedMilliseconds, statusCode);

                    // Format and log the message
                    _logger.Log(logLevel, 
                        "Request {Method} {Path}{Query} from {UserIp} ({User}) " +
                        "completed in {ElapsedMilliseconds}ms with status {StatusCode}",
                        requestMethod, requestPath, requestQuery, userIp, userInfo, 
                        elapsedMilliseconds, statusCode);
                }
            }
        }

        private bool ShouldExcludePath(string path)
        {
            if (_options.ExcludePaths == null)
                return false;

            foreach (var excludePath in _options.ExcludePaths)
            {
                if (path.StartsWith(excludePath.ToLower()))
                    return true;
            }

            return false;
        }

        private LogLevel DetermineLogLevel(long elapsedMilliseconds, int statusCode)
        {
            // Error status codes are always logged as errors
            if (statusCode >= 500)
                return LogLevel.Error;
            if (statusCode >= 400)
                return LogLevel.Warning;

            // Determine log level based on response time
            if (elapsedMilliseconds >= 3000)
                return LogLevel.Warning; // Slow requests (> 3s)
            if (elapsedMilliseconds >= 1000)
                return LogLevel.Information; // Moderately slow requests (1-3s)

            return LogLevel.Debug; // Normal requests
        }
    }

    // Extension methods for easier middleware registration
    public static class RequestTimingMiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestTiming(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestTimingMiddleware>();
        }

        public static IApplicationBuilder UseRequestTiming(
            this IApplicationBuilder builder,
            Action<RequestTimingOptions> configureOptions)
        {
            builder.ApplicationServices
                .GetService(typeof(IOptions<RequestTimingOptions>))
                .As<IOptions<RequestTimingOptions>>()
                .Value.Apply(configureOptions);

            return builder.UseMiddleware<RequestTimingMiddleware>();
        }
    }

    // Helper extension method
    internal static class ObjectExtensions
    {
        public static T Apply<T>(this T obj, Action<T> action)
        {
            action(obj);
            return obj;
        }

        public static T As<T>(this object obj) where T : class
        {
            return obj as T;
        }
    }
}