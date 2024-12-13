using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace CourseAI.Api.Swagger.Extensions;

public static class SwaggerExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    public static void AddCustomSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.ConfigureOptions<SwaggerConfiguration>();
        services.AddSwaggerGen();
    }

    /// <summary>
    ///   Register the NeerSwaggerUI middleware as custom alternative for default SwaggerUI middleware
    /// </summary>
    /// <param name="app">An <see cref="ApplicationBuilder"/> instance.</param>
    public static void UseCustomSwagger(this IApplicationBuilder app)
    {
        var swaggerOptions = app.ApplicationServices.GetRequiredService<IOptions<SwaggerConfigurationOptions>>().Value;
        if (!swaggerOptions.Enabled) return;

        var apiProvider = app.ApplicationServices.GetRequiredService<IApiVersionDescriptionProvider>();
        app.UseSwagger();
        app.UseSwaggerUI(swagger =>
        {
            foreach (var description in apiProvider.ApiVersionDescriptions)
            {
                swagger.SwaggerEndpoint(
                    url: $"/swagger/{description.GroupName}/swagger.json",
                    name: $"{swaggerOptions.Title} {description.GroupName}"
                );
            }

            swagger.DocExpansion(DocExpansion.List);
        });
    }
}