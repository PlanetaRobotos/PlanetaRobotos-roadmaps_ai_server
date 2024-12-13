using CourseAI.Api;
using CourseAI.Api.Core;
using CourseAI.Domain.Context;
using Microsoft.EntityFrameworkCore;

var logger = AppLoggerFactory.CreateLogger().ForContext<Program>();

try
{
    var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
    var isProduction = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production";

    var builder = WebApplication.CreateBuilder(args);
    logger.Debug("[startup] Configuring application builder..");
    Startup.ConfigureBuilder(builder);

    var app = builder.Build();
    logger.Debug("[startup] Applying database migrations..");
    // ApplyDbMigrations(app.Services);

    logger.Debug("[startup] Running web application..");
    Startup.ConfigureWebApp(app);

    app.Run(isProduction ? $"http://0.0.0.0:{port}" : null);
}
catch (Exception e)
{
    logger.Fatal(e, "Unhandled exception");
}
finally
{
    logger.Information("[shutdown] Application is now stopping..");
}

return;

void ApplyDbMigrations(IServiceProvider serviceProvider)
{
    try
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        dbContext.Database.Migrate();
    }
    catch (Exception e)
    {
        logger.Fatal(e, "An error occurred while applying database migrations");
    }
}