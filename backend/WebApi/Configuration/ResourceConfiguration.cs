using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using WebApi.Resources;

namespace WebApi.Configuration
{
    /// <summary>
    /// Конфигурация ресурсов локализации
    /// </summary>
    public static class ResourceConfiguration
    {
        /// <summary>
        /// Добавляет службы локализации для приложения
        /// </summary>
        public static IServiceCollection AddLocalizationServices(this IServiceCollection services)
        {
            // Настройка локализации
            services.AddLocalization(options => options.ResourcesPath = "Resources");

            // Настраиваем опции локализации
            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new List<CultureInfo>
                {
                    new CultureInfo("en"),
                    new CultureInfo("ru"),
                    new CultureInfo("ky")
                };

                options.DefaultRequestCulture = new RequestCulture("ru");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
                
                // Добавляем провайдеры для определения культуры
                options.RequestCultureProviders = new List<IRequestCultureProvider>
                {
                    new QueryStringRequestCultureProvider(),
                    new CookieRequestCultureProvider(),
                    new AcceptLanguageHeaderRequestCultureProvider()
                };
            });

            // Регистрируем сервис локализации для валидаций
            services.AddSingleton<IValidationLocalizerService, ValidationLocalizerService>();

            // Add HttpContextAccessor for localization service
            services.AddHttpContextAccessor();

            return services;
        }

        /// <summary>
        /// Настраивает middleware локализации для приложения
        /// </summary>
        public static IApplicationBuilder UseLocalizationConfiguration(this IApplicationBuilder app)
        {
            var localizationOptions = app.ApplicationServices.GetRequiredService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(localizationOptions.Value);

            return app;
        }
    }
}