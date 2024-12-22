// Application/Features/Users/ExternalLogin/InitiateGoogleLoginHandler.cs

using System.Threading;
using System.Threading.Tasks;
using CourseAI.Application.Core;
using CourseAI.Application.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OneOf;

namespace CourseAI.Application.Features.Users.ExternalLogin
{
    public class InitiateGoogleLoginHandler(IUrlHelper urlHelper)
        : IHandler<InitiateGoogleLoginRequest, AuthenticationProperties>
    {
        public async ValueTask<OneOf<AuthenticationProperties, Error>> Handle(InitiateGoogleLoginRequest request, CancellationToken ct)
        {
            var redirectUrl = urlHelper.Action(
                action: "ExternalLoginCallback",
                controller: "Auth",
                values: new { returnUrl = request.ReturnUrl },
                protocol: "https");

            var properties = new AuthenticationProperties { RedirectUri = redirectUrl,
                Items =
                {
                    ["scheme"] = "Google",
                },
            };

            return properties;
        }
    }
}
