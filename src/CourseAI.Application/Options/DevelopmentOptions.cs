using CourseAI.Core.Constants;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace CourseAI.Application.Options;

public class DevelopmentOptions
{
    public bool EnableSwagger { get; set; }


    internal class Configuration(IConfiguration configuration) : IConfigureOptions<DevelopmentOptions>
    {
        public void Configure(DevelopmentOptions options)
        {
            var config = configuration.GetRequiredSection(ConfigSectionNames.Development);
            config.Bind(options);
        }
    }
}