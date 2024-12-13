using System.Text;
using Fleet.Core.Constants;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Fleet.Application.Options;

public class JwtOptions
{
    public AccessTokenOptions AccessToken { get; init; } = new();
    public RefreshTokenOptions RefreshToken { get; init; } = new();


    internal class Configuration(IConfiguration configuration) : IConfigureOptions<JwtOptions>
    {
        public void Configure(JwtOptions options)
        {
            var config = configuration.GetRequiredSection(ConfigSectionNames.Jwt);
            config.Bind(options);

            var stringToken = config.GetValue<string>("AccessToken:Secret")!;
            options.AccessToken.Secret = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(stringToken));
        }
    }
}

public class AccessTokenOptions
{
    public SecurityKey Secret { get; internal set; } = null!;
    public string? Issuer { get; init; }
    public string[]? Audiences { get; init; }
    public TimeSpan Lifetime { get; init; }
    public TimeSpan ClockSkew { get; init; }
}

public class RefreshTokenOptions
{
    public TimeSpan Lifetime { get; init; }
    public string CookieName { get; init; } = null!;
    public string CookieDomain { get; init; } = null!;
}