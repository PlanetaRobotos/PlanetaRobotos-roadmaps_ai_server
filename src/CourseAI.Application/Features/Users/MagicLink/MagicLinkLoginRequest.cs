using CourseAI.Application.Core;
using Mediator;

namespace CourseAI.Application.Features.Users.MagicLink;

public class MagicLinkLoginRequest : IRequestModel<string>
{
    public Guid TokenId { get; set; }
}
