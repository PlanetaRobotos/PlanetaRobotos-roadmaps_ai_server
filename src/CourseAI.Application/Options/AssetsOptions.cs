using CourseAI.Core.Constants;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace CourseAI.Application.Options;

public class AssetsOptions
{
    public string JobFieldsConfiguration { get; init; } = null!;
    public string JobFieldItemsConfiguration { get; init; } = null!;


    internal class Configuration(IConfiguration configuration) : IConfigureOptions<AssetsOptions>
    {
        public void Configure(AssetsOptions options)
        {
            var config = configuration.GetRequiredSection(ConfigSectionNames.Assets);
            config.Bind(options);
        }
    }
}