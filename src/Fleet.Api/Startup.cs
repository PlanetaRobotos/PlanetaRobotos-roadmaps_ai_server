using Fleet.Api.Middlewares;
using Fleet.Api.Swagger.Extensions;
using Fleet.Application;
using Fleet.Application.Extensions;
using Fleet.Application.Options;
using Fleet.Domain;
using Fleet.Infrastructure;
using Serilog;

namespace Fleet.Api;

internal static class Startup
{
    public static void ConfigureBuilder(WebApplicationBuilder builder)
    {
        builder.Services.AddApplicationInsightsTelemetry();

        builder.Configuration.AddJsonFile("appsettings.Secrets.json", optional: false);
        builder.Host.UseSerilog();

        builder.Services.AddDomain();
        builder.Services.AddApplication();
        builder.Services.AddInfrastructure();
        builder.Services.AddWebApi();
    }

    public static void ConfigureWebApp(WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        var devOptions = app.Services.GetOptions<DevelopmentOptions>().Value;
        // if (devOptions.EnableSwagger)
        {
            app.UseCustomSwagger();
            // app.UseSwagger();
            // app.UseSwaggerUI(c =>
            // {
            //     c.SwaggerEndpoint("/swagger/v1/swagger.json", "Fleer API v1");
            // });
            // app.ForceRedirect(from: "/", to: "/swagger/index.html");
        }

        app.UseCors("AcceptAll");

        if (app.Environment.IsProduction())
        {
            // app.UseResponseCaching();
            app.UseHttpsRedirection();
        }

        app.UseSerilogRequestLogging();

        app.UseMiddleware<CustomExceptionHandler>();

        // app.MapIdentityApiWithCustomRoutes<User>();
        app.MapControllers();

        // app.MapGet("/", () => "OK");
    }
}