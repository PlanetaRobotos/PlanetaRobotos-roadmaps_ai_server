using System.Net.Http.Headers;
using Fleet.Application.Services;
using Fleet.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using NeerCore.DependencyInjection.Extensions;

namespace Fleet.Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services)
    {
        services.AddHttpClient();

        // services.AddHttpClient<IAiContentGenerator, OpenAiContentGenerator>(client =>
        // {
        //     client.BaseAddress = new Uri("https://api.openai.com/v1/");
        //     client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "YOUR_OPENAI_API_KEY");
        // });

        services.AddAllServices(options =>
        {
            options.DefaultLifetime = ServiceLifetime.Transient;
            options.ResolveInternalImplementations = true;
        });
    }
}