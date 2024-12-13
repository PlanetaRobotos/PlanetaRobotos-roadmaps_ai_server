using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NeerCore.Data.EntityFramework.Design;
using NeerCore.DependencyInjection.Extensions;

namespace CourseAI.Domain.Context;

public class AppDbContextFactory : DbContextFactoryBase<AppDbContext>
{
    public IConfiguration? Configuration { get; init; }

    public override string SelectedConnectionName => "Default";

    public override string[] SettingsPaths =>
    [
        "appsettings.Secrets.json",
        $"../{GetType().Assembly.GetBaseNamespace()}.Api/appsettings.Secrets.json",
    ];

    public override string ConnectionString
    {
        get
        {
            if (Configuration is null)
                return base.ConnectionString;

            return Configuration.GetConnectionString(SelectedConnectionName)
                   ?? throw new KeyNotFoundException("Connection string not found in configuration");
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