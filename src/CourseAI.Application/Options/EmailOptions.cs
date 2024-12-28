using CourseAI.Core.Constants;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace CourseAI.Application.Options;

public class EmailOptions
{
    public string SenderEmail { get; set; } = null!;
    public string Sender { get; set; } = null!;
    public string Host { get; set; } = null!;
    public int Port { get; set; }
    public bool EnableSsl { get; set; }
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
    
    internal class Configuration(IConfiguration configuration) : IConfigureOptions<EmailOptions>
    {
        public void Configure(EmailOptions options)
        {
            var config = configuration.GetRequiredSection(ConfigSectionNames.Email);
            config.Bind(options);
        }
    }
}