using CourseAI.Core.Security;
using CourseAI.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace CourseAI.Api.Extensions;

public static class UserSeeder
{
    public static async Task SeedAdminUserAsync(IServiceProvider serviceProvider)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
        var roleManager = serviceProvider.GetRequiredService<RoleManager<Role>>();

        string adminEmail = "admin@example.com";
        string adminPassword = "Admin@123"; // Ensure this meets password policies

        var adminUser = await userManager.FindByEmailAsync(adminEmail);

        if (adminUser == null)
        {
            var newAdmin = new User
            {
                UserName = "admin",
                Email = adminEmail,
                EmailConfirmed = true // Set to true if email confirmation is not required
            };

            var createAdminResult = await userManager.CreateAsync(newAdmin, adminPassword);
            if (createAdminResult.Succeeded)
            {
                // Assign Admin role
                var addRoleResult = await userManager.AddToRoleAsync(newAdmin, Roles.admin.ToString());
                if (!addRoleResult.Succeeded)
                {
                    throw new Exception("Failed to assign Admin role to the admin user.");
                }
            }
            else
            {
                throw new Exception("Failed to create admin user.");
            }
        }
        else
        {
            // Ensure the user has the Admin role
            if (!await userManager.IsInRoleAsync(adminUser, Roles.admin.ToString()))
            {
                await userManager.AddToRoleAsync(adminUser, Roles.admin.ToString());
            }
        }
    }
}