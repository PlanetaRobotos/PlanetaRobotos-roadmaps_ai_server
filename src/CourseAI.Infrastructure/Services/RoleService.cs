using CourseAI.Application.Models;
using CourseAI.Application.Services;
using CourseAI.Domain.Entities.Identity;
using FluentEmail.Core;
using Microsoft.AspNetCore.Identity;
using NeerCore.DependencyInjection;
using OneOf.Types;
using Error = CourseAI.Application.Models.Error;

namespace CourseAI.Infrastructure.Services;

[Service]
public class RoleService(UserManager<User> userManager) : IRoleService
{
    public async Task<bool> AssignRoleAsync(long userId, string roleName)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());
        if (user == null)
            return false;

        var result = await userManager.AddToRoleAsync(user, roleName);
        if(result.Succeeded)
            return true;

        foreach (var error in result.Errors) 
            Error.ServerError($"Error assign role: {error}");
        return false;
    }

    public async Task<bool> RemoveRoleAsync(long userId, string roleName)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());
        if (user == null)
            return false;

        var result = await userManager.RemoveFromRoleAsync(user, roleName);
        if(result.Succeeded)
            return true;
        
        foreach (var error in result.Errors)
            Error.ServerError($"Error remove role: {error}");
        return false;
    }
}