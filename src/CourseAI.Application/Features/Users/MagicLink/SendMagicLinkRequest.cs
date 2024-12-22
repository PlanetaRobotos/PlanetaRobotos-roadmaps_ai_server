// Application/Features/Users/MagicLink/SendMagicLinkRequest.cs
using CourseAI.Application.Core;
using Mediator;

namespace CourseAI.Application.Features.Users.MagicLink
{
    public class SendMagicLinkRequest : IRequestModel
    {
        public string Email { get; set; }
    }
}