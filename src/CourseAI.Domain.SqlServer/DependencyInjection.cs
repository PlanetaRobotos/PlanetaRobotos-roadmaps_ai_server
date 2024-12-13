using CourseAI.Core.Constants;
using CourseAI.Domain.Context;
using CourseAI.Domain.Entities.Identity;
using CourseAI.Domain.Sieve;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sieve.Services;

namespace CourseAI.Domain;

public static class DependencyInjection
{
    public static void AddDomain(this IServiceCollection services)
    {
        var configuration = services.BuildServiceProvider().GetRequiredService<IConfiguration>();

        services.AddDbContext(configuration);
        services.AddCustomIdentity(configuration);
        services.AddSieve();
    }

    private static void AddDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        var contextFactory = new AppDbContextFactory { Configuration = configuration };
        services.AddDbContext<AppDbContext>(options =>
        {
            contextFactory.ConfigureContextOptions(options);
        });
    }

    private static void AddCustomIdentity(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddIdentityCore<User>(configuration.GetRequiredSection(ConfigSectionNames.Identity).Bind)
            .AddRoles<Role>()
            .AddTokenProvider<EmailTokenProvider<User>>("Default")
            .AddEntityFrameworkStores<AppDbContext>();
    }

    private static void AddSieve(this IServiceCollection services)
    {
        var assembly = typeof(DependencyInjection).Assembly;
        var configurationTypes = assembly.GetTypes()
            .Where(t => typeof(ISieveConfiguration).IsAssignableFrom(t) && t is { IsInterface: false, IsAbstract: false });

        foreach (var type in configurationTypes)
        {
            services.AddScoped(typeof(ISieveConfiguration), type);
        }

        services.AddScoped<ISieveCustomSortMethods, CustomSortMethods>();
        services.AddScoped<ISieveCustomFilterMethods, CustomFilterMethods>();
        services.AddScoped<ISieveProcessor, CustomSieveProcessor>();
    }
}