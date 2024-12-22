using CourseAI.Application.Core;

namespace CourseAI.Application.Features.Users.ExternalLogin;

public class ExternalLoginCallbackRequest : IRequestModel<string>
{
    public string Provider { get; set; }
    public string ReturnUrl { get; set; }
    public string RemoteError { get; set; }
}
