using CourseAI.Core.Constants;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace CourseAI.Application.Options;

public class S3Options
{
    public string BucketName { get; set; } = string.Empty;
    public string Region { get; set; } = string.Empty;
    public string AccessKey { get; set; } = string.Empty;
    public string SecretKey { get; set; } = string.Empty;
    
    internal class Configuration(IConfiguration configuration) : IConfigureOptions<S3Options>
    {
        public void Configure(S3Options options)
        {
            var config = configuration.GetRequiredSection(ConfigSectionNames.S3);
            config.Bind(options);
        }
    }
}