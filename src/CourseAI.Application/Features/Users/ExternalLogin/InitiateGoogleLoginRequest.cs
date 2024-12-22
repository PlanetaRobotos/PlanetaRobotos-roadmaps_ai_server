using CourseAI.Application.Core;
using Microsoft.AspNetCore.Authentication;

namespace CourseAI.Application.Features.Users.ExternalLogin
{
    public class InitiateGoogleLoginRequest : IRequestModel<AuthenticationProperties>
    {
        public string ReturnUrl { get; set; }
    }
}