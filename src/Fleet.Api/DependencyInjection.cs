using Asp.Versioning;
using Fleet.Api.Core;
using Fleet.Api.Core.ModelBinding;
using Fleet.Api.Middlewares;
using Fleet.Api.Swagger;
using Fleet.Api.Swagger.Extensions;
using Fleet.Core.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Fleet.Api;

public static class DependencyInjection
{
    public static void AddWebApi(this IServiceCollection services)
    {
        var configuration = services.BuildServiceProvider().GetRequiredService<IConfiguration>();

        services.AddScoped<CustomExceptionHandler>();
        services.Configure<SwaggerConfigurationOptions>(configuration.GetSection(ConfigSectionNames.Swagger).Bind);

        services.AddControllers(mvc =>
        {
            mvc.Conventions.Add(new RouteTokenTransformerConvention(new KebabCaseNamingConvention()));
            mvc.AddFromBodyOrRouteModelBinder(services);
        }).AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new CustomJsonStringEnumConverter());
        });

        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });

        services.AddCors(cors => cors.AddPolicy("AcceptAll", builder =>
            builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
        ));

        services.AddProblemDetails();
        services.AddCustomApiVersioning();
        services.AddCustomSwagger();
    }

    private static void AddCustomApiVersioning(this IServiceCollection services)
    {
        services.AddApiVersioning(options =>
        {
            // options.DefaultApiVersion = new ApiVersion(1);
            // options.AssumeDefaultVersionWhenUnspecified = false;
            options.ApiVersionReader = new UrlSegmentApiVersionReader();
        }).AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'V";
            options.SubstituteApiVersionInUrl = true;
        });
    }
}