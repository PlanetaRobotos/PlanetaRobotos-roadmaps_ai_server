using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace CourseAI.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IOptions<TOptions> GetOptions<TOptions>(this IServiceCollection services)
        where TOptions : class
    {
        var serviceProvider = services.BuildServiceProvider();
        return serviceProvider.GetOptions<TOptions>();
    }

    public static IOptions<TOptions> GetOptions<TOptions>(this IServiceProvider serviceProvider)
        where TOptions : class
    {
        return serviceProvider.GetRequiredService<IOptions<TOptions>>();
    }
}