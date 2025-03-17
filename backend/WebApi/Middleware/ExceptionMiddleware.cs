using System.Net;
using System.Text.Json;
using Application.Exceptions;
using Domain;
using FluentResults;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace WebApi.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _environment;

        public ExceptionMiddleware(
            RequestDelegate next, 
            ILogger<ExceptionMiddleware> logger,
            IHostEnvironment environment)
        {
            _next = next;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _environment = environment;
        }
        
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
                
                // Handle error status codes that may have been set before this middleware
                if (!context.Response.HasStarted && context.Response.StatusCode >= 400)
                {
                    await HandleErrorStatusCodeAsync(context);
                }
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }
        
        private async Task HandleErrorStatusCodeAsync(HttpContext context)
        {
            var statusCode = context.Response.StatusCode;
            var errorResponse = statusCode switch
            {
                StatusCodes.Status400BadRequest => CreateErrorResponse("Bad Request", "INVALID_REQUEST", "The request was invalid or cannot be served."),
                StatusCodes.Status401Unauthorized => CreateErrorResponse("Unauthorized", "UNAUTHORIZED", "Authentication is required and has failed or has not been provided."),
                StatusCodes.Status403Forbidden => CreateErrorResponse("Forbidden", "FORBIDDEN", "You don't have permission to access this resource."),
                StatusCodes.Status404NotFound => CreateErrorResponse("Not Found", "NOT_FOUND", "The requested resource could not be found."),
                StatusCodes.Status422UnprocessableEntity => CreateErrorResponse("Unprocessable Entity", "VALIDATION_FAILED", "The request was well-formed but was unable to be followed due to semantic errors."),
                StatusCodes.Status500InternalServerError => CreateErrorResponse("Internal Server Error", "INTERNAL_ERROR", "An unexpected condition was encountered."),
                _ => CreateErrorResponse("Error", "UNKNOWN_ERROR", $"An error occurred with status code {statusCode}")
            };

            LogHttpError(context, statusCode, errorResponse);
            await WriteErrorResponseAsync(context, statusCode, errorResponse);
        }
        
        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var (statusCode, errorResponse) = exception switch
            {
                PermissionException ex => (StatusCodes.Status403Forbidden, 
                    CreateErrorResponse("Forbidden", "PERMISSION_DENIED", ex.Message, ex.Details)),
                
                ServiceException ex => (StatusCodes.Status503ServiceUnavailable, 
                    CreateErrorResponse("Service Unavailable", "SERVICE_ERROR", ex.Message)),
                
                RepositoryException ex => (StatusCodes.Status500InternalServerError, 
                    CreateErrorResponse("Database Error", "REPOSITORY_ERROR", ex.Message)),
                
                // Handle FluentValidation exceptions if you're using it
                // FluentValidation.ValidationException ex => (StatusCodes.Status422UnprocessableEntity,
                //     CreateValidationErrorResponse(ex)),
                
                // Handle CustomError from Domain model
                Exception ex when ex.Data.Contains("CustomError") => 
                    HandleCustomErrorException(ex),
                
                _ => (StatusCodes.Status500InternalServerError, 
                    CreateErrorResponse("Internal Server Error", "UNEXPECTED_ERROR", 
                        _environment.IsDevelopment() ? exception.Message : "An unexpected error occurred."))
            };
            
            LogException(exception, statusCode, errorResponse);
            await WriteErrorResponseAsync(context, statusCode, errorResponse);
        }

        private (int StatusCode, ErrorResponse Response) HandleCustomErrorException(Exception ex)
        {
            var customError = (CustomError)ex.Data["CustomError"];
            var statusCode = customError.StatusCode;
            
            return (statusCode, CreateErrorResponse(
                customError.ErrorType.ToString(), 
                customError.ErrorType.ToString(), 
                customError.Message,
                customError.Metadata.ContainsKey("Parameters") ? customError.Metadata["Parameters"] : null));
        }

        private void LogException(Exception exception, int statusCode, ErrorResponse errorResponse)
        {
            var logLevel = statusCode switch
            {
                StatusCodes.Status400BadRequest => LogLevel.Warning,
                StatusCodes.Status401Unauthorized => LogLevel.Warning,
                StatusCodes.Status403Forbidden => LogLevel.Warning,
                StatusCodes.Status404NotFound => LogLevel.Warning,
                StatusCodes.Status422UnprocessableEntity => LogLevel.Warning,
                _ => LogLevel.Error
            };

            var errorData = new
            {
                StatusCode = statusCode,
                ErrorType = errorResponse.Code,
                Message = errorResponse.Message,
                Exception = _environment.IsDevelopment() ? exception.ToString() : exception.Message,
                StackTrace = _environment.IsDevelopment() ? exception.StackTrace : null
            };
            
            _logger.Log(logLevel, exception, "Error occurred: {@ErrorDetails}", errorData);
        }
        
        private void LogHttpError(HttpContext context, int statusCode, ErrorResponse errorResponse)
        {
            var logLevel = statusCode switch
            {
                StatusCodes.Status400BadRequest => LogLevel.Warning,
                StatusCodes.Status401Unauthorized => LogLevel.Warning,
                StatusCodes.Status403Forbidden => LogLevel.Warning,
                StatusCodes.Status404NotFound => LogLevel.Warning,
                StatusCodes.Status422UnprocessableEntity => LogLevel.Warning,
                _ => LogLevel.Error
            };
            
            var path = context.Request.Path;
            var method = context.Request.Method;
            
            var errorData = new
            {
                StatusCode = statusCode,
                Path = path,
                Method = method,
                ErrorType = errorResponse.Code,
                Message = errorResponse.Message
            };
            
            _logger.Log(logLevel, "HTTP error response: {@ErrorDetails}", errorData);
        }

        private async Task WriteErrorResponseAsync(HttpContext context, int statusCode, ErrorResponse errorResponse)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            var response = new ApiResponse(statusCode, new[] { errorResponse }, null);
            await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
        }

        private static ErrorResponse CreateErrorResponse(string title, string code, string message, object details = null)
        {
            return new ErrorResponse(title, code, message, details);
        }
    }

    public record ErrorResponse(string Title, string Code, string Message, object Details = null);
    public record ApiResponse(int StatusCode, IEnumerable<ErrorResponse> Errors, IEnumerable<string> Success);
}