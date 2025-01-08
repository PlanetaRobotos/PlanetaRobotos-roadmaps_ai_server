using System.Security.Claims;
using CourseAI.Application.Extensions;
using CourseAI.Application.Models;
using CourseAI.Application.Services;
using CourseAI.Domain.Entities.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using NeerCore.DependencyInjection;
using OneOf;

namespace CourseAI.Infrastructure.Services;

[Service]
public class UserService(UserManager<User> userManager, IRoleService roleService, IHttpContextAccessor httpContextAccessor) : IUserService
{
    public async Task<User?> CreateUser(string email, bool emailConfirmed, string? role, int tokensAmount)
    {
        var user = new User
        {
            UserName = email,
            Email = email,
            FirstName = email.ToUsername(),
            EmailConfirmed = emailConfirmed
        };

        var identityResult = await userManager.CreateAsync(user);
        if (!identityResult.Succeeded)
        {
            foreach (var error in identityResult.Errors)
                Error.ServerError($"identityResult Failed: {error.Description}");
            return null;
        }

        if (role != null)
        {
            var assignResult = await roleService.AssignRoleAsync(user.Id, role);
            if (!assignResult)
                Error.ServerError("Failed to assign role.");
        }
        
        if (tokensAmount > 0)
        {
            user.Tokens += tokensAmount;
            var updateResult = await userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                foreach (var error in updateResult.Errors)
                    Error.ServerError($"updateResult Failed: {error.Description}");
            }
        }

        return user;
    }

    public async ValueTask<OneOf<User, Error>> GetUser()
    {
        var httpUser = httpContextAccessor.HttpContext?.User;
        if (httpUser == null)
            return Error.Unauthorized("User not found from httpContextAccessor");

        var userId = httpUser.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
            return Error.Unauthorized("User not found by ClaimTypes.NameIdentifier");
    
        var user = await userManager.FindByIdAsync(userId);
        if (user == null)
            return Error.Unauthorized("User not found from userManager");
        
        return user;
    }
}