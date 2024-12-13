using CourseAI.Domain.Entities.Identity;
using NeerCore.Data.EntityFramework.Abstractions;

namespace CourseAI.Domain.Seeders;

public class UserRolesSeeder : IEntityDataSeeder<UserRole>
{
    public IEnumerable<UserRole> Data => new UserRole[]
    {
        new()
        {
            UserId = 1,
            RoleId = 3,
        },
    };
}