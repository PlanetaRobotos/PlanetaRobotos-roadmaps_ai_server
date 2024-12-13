using System.Text;
using Fleet.Core.Constants;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Fleet.Application.Options;

public class OpenAIOptions
{
    public string ApiKey { get; set; } = null!;
    
    internal class Configuration(IConfiguration configuration) : IConfigureOptions<OpenAIOptions>
    {
        public void Configure(OpenAIOptions options)
        {
            var config = configuration.GetRequiredSection(ConfigSectionNames.OpenAI);
            config.Bind(options);
        }
    }
}
