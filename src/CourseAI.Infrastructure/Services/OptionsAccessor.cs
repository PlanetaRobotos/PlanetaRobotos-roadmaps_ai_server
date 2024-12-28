using CourseAI.Application.Options;
using CourseAI.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NeerCore.DependencyInjection;

namespace CourseAI.Infrastructure.Services;

[Service]
public class OptionsAccessor(IServiceProvider serviceProvider) : IOptionsAccessor
{
    private AssetsOptions? _assets;
    private JwtOptions? _jwt;
    private DevelopmentOptions? _development;
    private EmailOptions? _email;

    public AssetsOptions Assets => _assets ??= serviceProvider.GetRequiredService<IOptions<AssetsOptions>>().Value;
    public JwtOptions Jwt => _jwt ??= serviceProvider.GetRequiredService<IOptions<JwtOptions>>().Value;
    public DevelopmentOptions Development => _development ??= serviceProvider.GetRequiredService<IOptions<DevelopmentOptions>>().Value;
    public EmailOptions Email => _email ??= serviceProvider.GetRequiredService<IOptions<EmailOptions>>().Value;
}