using CourseAI.Application.Core;
using Mediator;

namespace CourseAI.Application.Features.Users.MagicLink;

public class MagicLinkLoginRequest : IRequestModel
{
    public long UserId { get; set; }
    public string Token { get; set; }
}
