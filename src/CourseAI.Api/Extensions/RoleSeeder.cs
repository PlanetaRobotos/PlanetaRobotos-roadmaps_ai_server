using CourseAI.Core.Security;
using CourseAI.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;

public static class RoleSeeder
{
    public static async Task SeedRolesAsync(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<Role>>();
        
        foreach (var roleName in Enum.GetNames(typeof(Roles)))
        {
            var roleExists = await roleManager.RoleExistsAsync(roleName);
            if (!roleExists)
            {
                var roleResult = await roleManager.CreateAsync(new Role { Name = roleName });
                if (!roleResult.Succeeded)
                {
                    throw new Exception($"Failed to create role: {roleName}");
                }
            }
        }
    }
}