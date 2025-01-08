using Asp.Versioning;
using CourseAI.Api.Core;
using CourseAI.Api.Core.ModelBinding;
using CourseAI.Api.Middlewares;
using CourseAI.Api.Swagger;
// using CourseAI.Api.Swagger.Extensions;
using CourseAI.Core.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Net.Http.Headers;

namespace CourseAI.Api;

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
            })
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new CustomJsonStringEnumConverter());
            })
            .AddFormatterMappings(options =>
            {
                options.SetMediaTypeMappingForFormat("form",
                    MediaTypeHeaderValue.Parse("application/x-www-form-urlencoded"));
            });

        services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });

        services.AddCors(cors => cors.AddPolicy("AcceptAll", builder =>
            builder.WithOrigins(
                    "http://localhost:3000",
                    "https://testroadmapai.vercel.app",
                    "https://www.levenue.tech"
                )
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()
        ));

        services.AddProblemDetails();
        services.AddCustomApiVersioning();
        // services.AddCustomSwagger();
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