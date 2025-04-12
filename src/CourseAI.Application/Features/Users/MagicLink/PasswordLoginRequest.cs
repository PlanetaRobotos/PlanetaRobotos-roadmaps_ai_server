using CourseAI.Application.Core;

namespace CourseAI.Application.Features.Users.MagicLink;

public class PasswordLoginRequest : IRequestModel<string>
{
    public string Email { get; set; }
    public string Password { get; set; }
}