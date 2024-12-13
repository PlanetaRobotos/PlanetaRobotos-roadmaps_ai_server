using System.Security.Claims;

namespace CourseAI.Core.Security;

public static class Claims
{
    public const string Id = "sub";
    public const string UserName = ClaimTypes.Name;
    public const string Role = ClaimTypes.Role;
}