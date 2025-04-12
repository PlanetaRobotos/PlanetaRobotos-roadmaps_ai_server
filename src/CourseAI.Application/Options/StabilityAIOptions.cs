using CourseAI.Core.Constants;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace CourseAI.Application.Options;

public class StabilityAIOptions
{
    public string ApiKey { get; set; } = string.Empty;
    public bool IsEnabled { get; set; }
    
    internal class Configuration(IConfiguration configuration) : IConfigureOptions<StabilityAIOptions>
    {
        public void Configure(StabilityAIOptions options)
        {
            var config = configuration.GetRequiredSection(ConfigSectionNames.StabilityAI);
            config.Bind(options);
        }
    }
}