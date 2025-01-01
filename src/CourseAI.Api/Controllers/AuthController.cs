using CourseAI.Api.Core;
using CourseAI.Api.Extensions;
using CourseAI.Application.Features.Users.ExternalLogin;
using CourseAI.Application.Features.Users.MagicLink;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;

namespace CourseAI.Api.Controllers;

public class AuthController(IConfiguration configuration) : V1Controller
{
    [HttpPost("send-magic-link")]
    [ProducesResponseType<string>(StatusCodes.Status201Created)]
    public async Task<ActionResult<string>> Create(SendMagicLinkRequest request)
    {
        var response = await Sender.Send(request);
        return response.MatchResponse(
            token => Ok(token));
    }

    [HttpGet("VerifyEmail", Name = "VerifyEmail")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    public async Task<IActionResult> VerifyEmail(Guid token)
    {
        var response = await Sender.Send(new MagicLinkLoginRequest { TokenId = token });
        return response.MatchResponse(
            token =>
            {
                string? client = configuration["Client:Url"];
                Logger.LogInformation("Redirecting to client {client} with token {token}", client, token);
                return Redirect($"{client}/dashboard?token={token}");
            });
    }

    [HttpGet("external-login/google")]
    public IActionResult InitiateGoogleLogin([FromQuery] string returnUrl = "/")
    {
        var redirectUrl = Url.Action(nameof(GoogleLoginCallback), new { returnUrl });
        var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
        return Challenge(properties, GoogleDefaults.AuthenticationScheme);
    }

    [HttpGet("external-login/google-callback")]
    public async Task<IActionResult> GoogleLoginCallback(string returnUrl = "/")
    {
        var response = await Sender.Send(new ExternalLoginCallbackRequest { ReturnUrl = returnUrl });

        return response.MatchResponse(
            token =>
            {
                string? client = configuration["Client:Url"];
                return Redirect($"{client}/dashboard?token={token}");
            });
    }
}