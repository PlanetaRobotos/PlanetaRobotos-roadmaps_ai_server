using CourseAI.Application.Core;
using Mediator;

namespace CourseAI.Application.Features.Users.MagicLink;

public class MagicLinkLoginRequest : IRequestModel<string>
{
    public Guid TokenId { get; set; }
}

public class VerifyEmailRequest
{
    public Guid Token { get; set; }
    public string? ReturnUrl { get; set; }
}