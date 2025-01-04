using CourseAI.Application.Core;

namespace CourseAI.Application.Features.Users.MagicLink;

public class RedirectRequest : IRequestModel<string>
{
    public string Email { get; set; }
}