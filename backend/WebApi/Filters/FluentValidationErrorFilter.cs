using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace WebApi.Filters
{
    /// <summary>
    /// Фильтр для преобразования ошибок валидации FluentValidation в стандартный формат ответа API
    /// </summary>
    public class FluentValidationErrorFilter : IExceptionFilter
    {
        private readonly ILogger<FluentValidationErrorFilter> _logger;

        public FluentValidationErrorFilter(ILogger<FluentValidationErrorFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            if (context.Exception is ValidationException validationException)
            {
                // Логируем ошибки валидации
                _logger.LogWarning("Ошибка валидации: {Path} {Method} - {Errors}",
                    context.HttpContext.Request.Path,
                    context.HttpContext.Request.Method,
                    string.Join(", ", validationException.Errors.Select(e => $"{e.PropertyName}: {e.ErrorMessage}")));

                // Преобразуем ошибки валидации в стандартный формат ответа
                var errors = validationException.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.ErrorMessage).ToArray()
                    );

                var problemDetails = new ValidationProblemDetails(errors)
                {
                    Status = (int)HttpStatusCode.BadRequest,
                    Title = "Ошибка валидации",
                    Detail = "Один или несколько параметров запроса недействительны",
                    Instance = context.HttpContext.Request.Path
                };

                // Устанавливаем результат
                context.Result = new BadRequestObjectResult(problemDetails);
                context.ExceptionHandled = true;
            }
        }
    }
}