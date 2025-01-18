using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NeerCore.Data.EntityFramework.Design;
using NeerCore.DependencyInjection.Extensions;
using System;
using Microsoft.Extensions.DependencyInjection;

namespace CourseAI.Domain.Context
{
    public class AppDbContextFactory : DbContextFactoryBase<AppDbContext>
    {
        public IConfiguration? Configuration { get; init; }

        public override string SelectedConnectionName => "Default";

        public override string[] SettingsPaths
        {
            get
            {
                var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";

                return new[]
                {
                    "appsettings.json", $"appsettings.{environment}.json", "appsettings.Secrets.json",
                    $"../{GetType().Assembly.GetBaseNamespace()}.Api/appsettings.Secrets.json",
                };
            }
        }

        public override string ConnectionString
        {
            get
            {
                if (Configuration is null)
                    return base.ConnectionString;

                return Environment.GetEnvironmentVariable("ConnectionStrings__Default") ??
                       Configuration.GetConnectionString(SelectedConnectionName);
            }
        }

        public override AppDbContext CreateDbContext(string[] args) => new(CreateContextOptions());

        public override void ConfigureContextOptions(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConnectionString, options => 
                    options.MigrationsAssembly(MigrationsAssembly));
        }
    }
}