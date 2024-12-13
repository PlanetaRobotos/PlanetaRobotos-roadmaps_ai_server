using Fleet.Application.Options;

namespace Fleet.Application.Services;

public interface IOptionsAccessor
{
    public AssetsOptions Assets { get; }
    public JwtOptions Jwt { get; }
    public DevelopmentOptions Development { get; }
}