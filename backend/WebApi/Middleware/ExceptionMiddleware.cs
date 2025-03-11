using System.Net;
using Application.Exceptions;
using FluentResults;
using Newtonsoft.Json;

namespace WebApi.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
                await TryHandleFluentResult(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }
        
        private async Task TryHandleFluentResult(HttpContext context)
        {
            if (context.Response.HasStarted || !context.Items.TryGetValue("FluentResult", out var resultObj))
            {
                return;
            }

            if (resultObj is IResultBase fluentResult)
            {
                await HandleResultAsync(context, fluentResult);
            }
        }
        
        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var (result, logLevel) = exception switch
            {
                PermissionException ex => (CreatePermissionError(ex), LogLevel.Error),
                ServiceException ex => (CreateServiceError(ex), LogLevel.Error),
                RepositoryException ex => (CreateRepositoryError(ex), LogLevel.Error),
                _ => (CreateUnexpectedError(exception), LogLevel.Critical)
            };

            _logger.Log(logLevel, exception, "Error occurred: {ErrorDetails}", 
                JsonConvert.SerializeObject(result.Errors));
            
            await HandleResultAsync(context, result);
        }

        private static Result CreatePermissionError(PermissionException ex) => Result.Fail(
            new Error(ex.Message)
                .CausedBy(ex)
                .WithMetadata("ErrorCode", "PERMISSION_DENIED")
                .WithMetadata("Details", ex.Details));

        private static Result CreateServiceError(ServiceException ex) => Result.Fail(
            new Error(ex.Message)
                .CausedBy(ex)
                .WithMetadata("ErrorCode", "SERVICE_ERROR"));

        private static Result CreateRepositoryError(RepositoryException ex) => Result.Fail(
            new Error(ex.Message)
                .CausedBy(ex)
                .WithMetadata("ErrorCode", "REPOSITORY_ERROR"));

        private static Result CreateUnexpectedError(Exception ex) => Result.Fail(
            new Error("An unexpected error occurred.")
                .CausedBy(ex)
                .WithMetadata("ErrorCode", "UNEXPECTED_ERROR"));

        private async Task HandleResultAsync(HttpContext context, IResultBase result)
        {
            if (result.IsSuccess && context.Response.HasStarted)
            {
                return;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = GetStatusCode(result);

            var response = new ApiResponse(
                context.Response.StatusCode,
                result.IsSuccess ? null : CreateErrorResponses(result.Errors),
                result.IsSuccess ? result.Successes.Select(s => s.Message) : null
            );

            await context.Response.WriteAsync(
                JsonConvert.SerializeObject(response));
        }

        private static int GetStatusCode(IResultBase result)
        {
            if (result.IsSuccess) return StatusCodes.Status200OK;

            var errorCode = result.Errors
                .Select(e => e.Metadata.GetValueOrDefault("ErrorCode")?.ToString())
                .FirstOrDefault();

            return errorCode switch
            {
                "NOT_FOUND" => StatusCodes.Status404NotFound,
                "PERMISSION_DENIED" => StatusCodes.Status403Forbidden,
                "SERVICE_ERROR" => StatusCodes.Status503ServiceUnavailable,
                _ => StatusCodes.Status400BadRequest
            };
        }

        private static IEnumerable<ErrorResponse> CreateErrorResponses(IEnumerable<IError> errors) =>
            errors.Select(e => new ErrorResponse(
                e.Message,
                e.Metadata.GetValueOrDefault("ErrorCode")?.ToString() ?? "UNKNOWN",
                e.Metadata.TryGetValue("Details", out var details) ? details : null));
    }

    public record ErrorResponse(string Message, string Code, object Details);
    public record ApiResponse(int StatusCode, IEnumerable<ErrorResponse> Errors, IEnumerable<string> Success);
}