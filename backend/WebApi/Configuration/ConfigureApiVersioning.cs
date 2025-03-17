using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using Asp.Versioning.ApiExplorer;

namespace WebApi.Configuration
{
    public static class ApiVersioningConfiguration
    {
        public static void ConfigureApiVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(options =>
                {
                    options.ApiVersionReader = new UrlSegmentApiVersionReader();
                    
                    options.DefaultApiVersion = new ApiVersion(1, 0);
                    
                    options.AssumeDefaultVersionWhenUnspecified = true;
                    
                    options.ReportApiVersions = true;
                })
                .AddApiExplorer(options =>
                {
                    options.GroupNameFormat = "'v'VVV";
                    
                    options.SubstituteApiVersionInUrl = true;
                });
        }
    }
}