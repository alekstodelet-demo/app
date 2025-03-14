using Domain;
using FluentResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApi.Extensions;

public static class ActionResultHelper
{
    public static IActionResult FromResult<T>(Result<T> result, ILogger logger)
    {
        return result.ToActionResult(logger);
    }

    public static IActionResult FromResult(Result result, ILogger logger)
    {
        return result.ToActionResult(logger);
    }

    public static IActionResult FromResult<T>(Result<T> result, int successStatusCode = StatusCodes.Status200OK)
    {
        if (result.IsSuccess)
        {
            return new ObjectResult(result.Value) { StatusCode = successStatusCode };
        }

        var firstError = result.Errors.FirstOrDefault();
        if (firstError is CustomError customError)
        {
            var errorResponse = new
            {
                Title = customError.ErrorType.ToString(),
                Code = customError.ErrorType.ToString(),
                Message = customError.Message,
                Details = customError.Metadata.ContainsKey("Parameters") ? customError.Metadata["Parameters"] : null
            };

            return new ObjectResult(new { Errors = new[] { errorResponse } })
            {
                StatusCode = customError.StatusCode
            };
        }

        // Generic error handling as fallback
        return new ObjectResult(new 
        { 
            Errors = new[] 
            { 
                new 
                { 
                    Title = "Error",
                    Code = "UNKNOWN", 
                    Message = firstError?.Message ?? "An unknown error occurred" 
                } 
            } 
        })
        {
            StatusCode = StatusCodes.Status500InternalServerError
        };
    }
}