using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using WebApi.Filters;

namespace WebApi.Configuration
{
    /// <summary>
    /// Конфигурация валидации для приложения
    /// </summary>
    public static class ValidationConfiguration
    {
        /// <summary>
        /// Добавляет FluentValidation в сервисы приложения
        /// </summary>
        public static IServiceCollection AddValidation(this IServiceCollection services)
        {
            // Регистрируем все валидаторы из сборки
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            
            // Настраиваем FluentValidation для MVC
            services.AddFluentValidationAutoValidation(config =>
            {
                // Отключаем встроенную модельную валидацию ASP.NET Core
                config.DisableDataAnnotationsValidation = true;
            });

            // Настраиваем поведение модельной валидации
            services.Configure<ApiBehaviorOptions>(options =>
            {
                // Кастомизируем ответ при ошибке модельной валидации
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errors = context.ModelState
                        .Where(e => e.Value.Errors.Count > 0)
                        .ToDictionary(
                            kvp => kvp.Key,
                            kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                        );

                    var problemDetails = new ValidationProblemDetails(errors)
                    {
                        Status = StatusCodes.Status400BadRequest,
                        Title = "Ошибка валидации",
                        Detail = "Один или несколько параметров запроса недействительны",
                        Instance = context.HttpContext.Request.Path
                    };

                    return new BadRequestObjectResult(problemDetails);
                };
            });

            // Регистрируем фильтр исключений для перехвата и форматирования ошибок валидации
            services.AddScoped<FluentValidationErrorFilter>();

            return services;
        }
    }
}