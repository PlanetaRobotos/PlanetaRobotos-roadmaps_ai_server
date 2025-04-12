// Application/Features/Users/MagicLink/SendMagicLinkRequest.cs
using CourseAI.Application.Core;
using Mediator;

namespace CourseAI.Application.Features.Users.MagicLink
{
    public class SendMagicLinkRequest : IRequestModel<string>
    {
        public long UserId { get; set; }
        public string ReturnUrl { get; set; }
    }
}