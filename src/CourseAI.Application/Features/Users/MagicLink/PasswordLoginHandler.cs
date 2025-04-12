using CourseAI.Application.Core;
using CourseAI.Application.Models;
using CourseAI.Application.Services;
using CourseAI.Domain.Context;
using CourseAI.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using OneOf;

namespace CourseAI.Application.Features.Users.MagicLink;

public class PasswordLoginHandler(AppDbContext dbContext, IJwtProvider jwtProvider, UserManager<User> userManager, ILogger<PasswordLoginHandler> logger) : IHandler<PasswordLoginRequest, string>
{
    public async ValueTask<OneOf<string, Error>> Handle(PasswordLoginRequest request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            logger.LogInformation("User with email {Email} not found", request.Email);
            return Error.ServerError($"User {request.Email} not found");
        }
        
        var jwtToken = jwtProvider.Create(user);
        return jwtToken;
    }
}