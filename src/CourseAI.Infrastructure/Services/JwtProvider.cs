using System.Security.Claims;
using System.Text;
using CourseAI.Application.Options;
using CourseAI.Application.Services;
using CourseAI.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using NeerCore.DependencyInjection;

namespace CourseAI.Infrastructure.Services;

[Service]
public class JwtProvider(IOptions<JwtOptions> options, UserManager<User> userManager) : IJwtProvider
{
    public string Create(User user)
    {
        var accessToken = options.Value.AccessToken;
        var credentials = new SigningCredentials(accessToken.Secret, SecurityAlgorithms.HmacSha256);

        var roles = userManager.GetRolesAsync(user).Result;

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email)
        };

        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMonths(3),
            SigningCredentials = credentials,
            Issuer = accessToken.Issuer,
            Audience = accessToken.Audiences[0]
        };

        var handler = new JsonWebTokenHandler();
        var token = handler.CreateToken(tokenDescriptor);
        return token;
    }
}