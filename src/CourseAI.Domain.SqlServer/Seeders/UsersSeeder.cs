using CourseAI.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using NeerCore.Data.EntityFramework.Abstractions;

namespace CourseAI.Domain.Seeders;

public class UsersSeeder : IEntityDataSeeder<User>
{
    private static readonly PasswordHasher<User> Hasher = new();

    public IEnumerable<User> Data => new User[]
    {
        new()
        {
            Id = 1,
            UserName = "arkode",
            NormalizedUserName = "ARKODE",
            Email = "admin@mrCourseAI.dev",
            NormalizedEmail = "ADMIN@MRCourseAI.DEV",
            EmailConfirmed = true,
            PasswordHash = Hasher.HashPassword(null!, "qwerty123!"),
            SecurityStamp = Guid.NewGuid().ToString()
        },
    };
}