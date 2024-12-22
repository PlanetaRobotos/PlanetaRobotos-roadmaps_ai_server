using CourseAI.Application.Extensions;
using CourseAI.Application.Options;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace CourseAI.Api.Extensions;

public static class JwtAuthenticationExtensions
{
    public static AuthenticationBuilder AddJwtAuthentication(this IServiceCollection services)
    {
        var options = services.GetOptions<JwtOptions>().Value.AccessToken;

        return services.AddAuthentication(authOptions =>
            {
                authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                // authOptions.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(jwt =>
            {
                jwt.RequireHttpsMetadata = false;

                jwt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = options.Issuer is not null,
                    ValidIssuer = options.Issuer,
                    ValidateAudience = options.Audiences is not null,
                    ValidAudiences = options.Audiences,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = options.Secret,
                    // ValidateLifetime = true,
                    // Allowed lifetime extra
                    ClockSkew = options.ClockSkew == TimeSpan.Zero
                        ? TimeSpan.FromMinutes(5)
                        : options.ClockSkew,
                };
            });
    }
}