using CourseAI.Api.Core;
using CourseAI.Api.Extensions;
using CourseAI.Application.Features.Users.ExternalLogin;
using CourseAI.Application.Features.Users.MagicLink;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;

namespace CourseAI.Api.Controllers;

public class AuthController : V1Controller
{
    // Send Magic Link
    [HttpPost("magic-link")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> SendMagicLink(SendMagicLinkRequest request)
    {
        var response = await Sender.Send(request);
        return response.MatchEmptyResponse();
    }

    // Handle Magic Link Login
    [HttpGet("magic-link-login")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    public async Task<IActionResult> MagicLinkLogin(long userId, string token)
    {
        var response = await Sender.Send(new MagicLinkLoginRequest { UserId = userId, Token = token });
        return response.MatchEmptyResponse();
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
                /*Response.Cookies.Append("token", token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = false, // Set to true in production
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddHours(1),
                    Path = "/"
                });*/

                return Redirect($"{returnUrl}/callback?token={token}");
            });
    }
    
    // Logout endpoint to clear the cookie
    /*[HttpPost("logout")]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("token", new CookieOptions
        {
            HttpOnly = true,
            Secure = false, // Set to true in production
            SameSite = SameSiteMode.Strict,
            Path = "/"
        });

        return Ok(new { message = "Logged out successfully" });
    }*/
}