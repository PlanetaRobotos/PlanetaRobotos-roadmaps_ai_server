using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NeerCore.Data.EntityFramework.Design;
using NeerCore.DependencyInjection.Extensions;
using System;

namespace CourseAI.Domain.Context
{
    public class AppDbContextFactory : DbContextFactoryBase<AppDbContext>
    {
        public IConfiguration? Configuration { get; init; }

        public override string SelectedConnectionName => "Default";

        // Dynamically include environment-specific settings
        public override string[] SettingsPaths
        {
            get
            {
                // Retrieve the current environment; default to "Production" if not set
                var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";

                return new[]
                {
                    "appsettings.json", // Base settings
                    $"appsettings.{environment}.json", // Environment-specific settings
                    "appsettings.Secrets.json", // Common secrets
                    $"../{GetType().Assembly.GetBaseNamespace()}.Api/appsettings.Secrets.json", // Additional secrets
                };
            }
        }

        public override string ConnectionString
        {
            get
            {
                if (Configuration is null)
                    return base.ConnectionString;

                return Environment.GetEnvironmentVariable("ConnectionStrings__Default") ?? Configuration.GetConnectionString(SelectedConnectionName);
            }
        }

        public override AppDbContext CreateDbContext(string[] args) => new(CreateContextOptions());

        public override void ConfigureContextOptions(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConnectionString, options =>
            {
                options.MigrationsAssembly(MigrationsAssembly);
            });
        }
    }
}