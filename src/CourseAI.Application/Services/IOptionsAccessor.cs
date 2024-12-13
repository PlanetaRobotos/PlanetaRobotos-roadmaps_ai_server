using CourseAI.Application.Options;

namespace CourseAI.Application.Services;

public interface IOptionsAccessor
{
    public AssetsOptions Assets { get; }
    public JwtOptions Jwt { get; }
    public DevelopmentOptions Development { get; }
}