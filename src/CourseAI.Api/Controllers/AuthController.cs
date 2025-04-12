using CourseAI.Api.Core;
using CourseAI.Api.Extensions;
using CourseAI.Application.Features.Users.ExternalLogin;
using CourseAI.Application.Features.Users.MagicLink;
using CourseAI.Application.Services;
using CourseAI.Domain.Entities.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CourseAI.Api.Controllers;

public class AuthController(IConfiguration configuration, IJwtProvider jwtProvider, UserManager<User> userManager)
    : V1Controller
{
    [HttpPost("send-magic-link")]
    [ProducesResponseType<string>(StatusCodes.Status201Created)]
    public async Task<ActionResult<string>> Create(SendMagicLinkRequest request)
    {
        var response = await Sender.Send(request);
        return response.MatchResponse(email => Ok(email));
    }

    [HttpGet("redirect")]
    public async Task<ActionResult<string>> RedirectToDashboard([FromQuery] RedirectRequest request)
    {
        var response = await Sender.Send(request);
        return response.MatchResponse(
            token =>
            {
                string? client = configuration["Client:Url"];
                Logger.LogInformation("Redirecting to client {client} with token {token}", client, token);
                return Redirect($"{client}/dashboard?token={token}");
            });
    }

    [HttpPost("create-token/{userId:long}")]
    [ProducesResponseType<string>(StatusCodes.Status201Created)]
    public async Task<ActionResult<string>> CreateToken(long userId)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());
        if (user == null)
            return NotFound("User not found");

        var token = jwtProvider.Create(user);
        return Ok(token);
    }

    [HttpPost("password-login")]
    public async Task<ActionResult> PasswordLogin(PasswordLoginRequest request)
    {
        var response = await Sender.Send(request);
        return response.MatchResponse(
            token => Ok(new { token }));
    }

    [HttpGet("VerifyEmail", Name = "VerifyEmail")]
    [ProducesResponseType(typeof(VerifyEmailRequest), StatusCodes.Status200OK)]
    public async Task<IActionResult> VerifyEmail([FromQuery] VerifyEmailRequest request)
    {
        var response = await Sender.Send(new MagicLinkLoginRequest { TokenId = request.Token });
        return response.MatchResponse(
            token =>
            {
                string? client = configuration["Client:Url"];
                Logger.LogInformation("Redirecting to client {client} with token {token}", client, token);
                return Redirect($"{client}{request.ReturnUrl}?token={token}");
            });
    }

    [HttpGet("external-login/google")]
    public IActionResult InitiateGoogleLogin([FromQuery] string returnUrl = "/")
    {
        Logger.LogInformation("Google login callback with return url {returnUrl}", returnUrl);

        var redirectUrl = Url.Action(nameof(GoogleLoginCallback), new { returnUrl });
        var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
        return Challenge(properties, GoogleDefaults.AuthenticationScheme);
    }

    [HttpGet("external-login/google-callback")]
    public async Task<IActionResult> GoogleLoginCallback(string returnUrl = "/")
    {
        Logger.LogInformation("google-callback login callback with return url {returnUrl}", returnUrl);
        
        var response = await Sender.Send(new ExternalLoginCallbackRequest { ReturnUrl = returnUrl });

        return response.MatchResponse(
            token =>
            {
                string? client = configuration["Client:Url"];
                return Redirect($"{client}{returnUrl}?token={token}");
            });
    }
}