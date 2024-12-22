using CourseAI.Core.Constants;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace CourseAI.Application.Options;

public class MailgunOptions
{
    public string ApiKey { get; set; } = null!;
    public string Domain { get; set; } = null!;
    
    internal class Configuration(IConfiguration configuration) : IConfigureOptions<MailgunOptions>
    {
        public void Configure(MailgunOptions options)
        {
            var config = configuration.GetRequiredSection(ConfigSectionNames.Mailgun);
            config.Bind(options);
        }
    }
}
