using CourseAI.Application.Core;
using CourseAI.Application.Services;
using CourseAI.Core.Constants;
using CourseAI.Core.Security;
using CourseAI.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using OneOf;
using Error = CourseAI.Application.Models.Error;

namespace CourseAI.Application.Features.Tokens;

public class TokensGetBalanceHandler(IUserService userService, UserManager<User> userManager) : IHandler<TokensGetBalanceRequest, int>
{
    public async ValueTask<OneOf<int, Error>> Handle(TokensGetBalanceRequest request, CancellationToken ct)
    {
        var userResult = await userService.GetUser();
        var user = userResult.Match(
            user => user,
            error => throw new Exception(error.Message)
        );
        
        var roles = await userManager.GetRolesAsync(user);

        if (roles.Contains(Roles.Standard) || roles.Contains(Roles.Enterprise))
        {
            return SharedConstants.InfinityValue;
        }

        return user.Tokens;
    }
}