using System.Net.Http.Headers;
using CourseAI.Application.Services;
using CourseAI.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using NeerCore.DependencyInjection.Extensions;

namespace CourseAI.Infrastructure;

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