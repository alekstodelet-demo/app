using FluentResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WebApi.Extensions
{
    public static class ResultHandlerExtension
    {
        public static IActionResult ToActionResult<T>(this Result<T> result, ILogger logger)
        {
            if (result.IsSuccess)
            {
                return new OkObjectResult(result.Value);
            }

            return HandleErrors(result, logger);
        }

        public static IActionResult ToActionResult(this Result result, ILogger logger)
        {
            if (result.IsSuccess)
            {
                return new OkResult();
            }

            return HandleErrors(result, logger);
        }

        private static IActionResult HandleErrors(IResultBase result, ILogger logger)
        {
            var statusCode = GetStatusCode(result);
            var errorResponses = result.Errors.Select(MapToErrorResponse).ToList();
            
            // Log the errors
            LogErrors(result.Errors, statusCode, logger);

            return new ObjectResult(new { Errors = errorResponses })
            {
                StatusCode = statusCode
            };
        }

        private static void LogErrors(IEnumerable<IError> errors, int statusCode, ILogger logger)
        {
            var logLevel = statusCode >= 500 ? LogLevel.Error : LogLevel.Warning;

            foreach (var error in errors)
            {
                // Extract more details if it's an exceptional error
                if (error is ExceptionalError exceptionalError && exceptionalError.Exception != null)
                {
                    logger.Log(logLevel, exceptionalError.Exception, 
                        "Error occurred with code {ErrorCode}: {ErrorMessage}", 
                        error.Metadata.GetValueOrDefault("ErrorCode"), 
                        error.Message);
                }
                else
                {
                    logger.Log(logLevel, 
                        "Error occurred with code {ErrorCode}: {ErrorMessage}", 
                        error.Metadata.GetValueOrDefault("ErrorCode"), 
                        error.Message);
                }
            }
        }

        private static int GetStatusCode(IResultBase result)
        {
            if (result.IsSuccess) return StatusCodes.Status200OK;

            // Get the first error code to determine status code
            var errorCode = result.Errors
                .Select(e => e.Metadata.GetValueOrDefault("ErrorCode")?.ToString())
                .FirstOrDefault(c => !string.IsNullOrWhiteSpace(c));

            return errorCode switch
            {
                "NOT_FOUND" => StatusCodes.Status404NotFound,
                "VALIDATION" => StatusCodes.Status400BadRequest,
                "VALIDATION_FAILED" => StatusCodes.Status422UnprocessableEntity,
                "LOGIC" => StatusCodes.Status422UnprocessableEntity,
                "PERMISSION_ACCESS" => StatusCodes.Status403Forbidden,
                "PERMISSION_DENIED" => StatusCodes.Status403Forbidden,
                "UNAUTHORIZED" => StatusCodes.Status401Unauthorized,
                "ALREADY_UPDATED" => StatusCodes.Status409Conflict,
                "SERVICE_ERROR" => StatusCodes.Status503ServiceUnavailable,
                "REPOSITORY_ERROR" => StatusCodes.Status500InternalServerError,
                _ => StatusCodes.Status400BadRequest
            };
        }

        private static object MapToErrorResponse(IError error)
        {
            return new
            {
                Title = error.Metadata.GetValueOrDefault("ErrorType")?.ToString() ?? "Error",
                Code = error.Metadata.GetValueOrDefault("ErrorCode")?.ToString() ?? "UNKNOWN_ERROR",
                Message = error.Message,
                Details = error.Metadata.GetValueOrDefault("Details")
            };
        }
    }
}