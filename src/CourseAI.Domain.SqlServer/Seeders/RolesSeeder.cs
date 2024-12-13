using CourseAI.Domain.Entities.Identity;
using NeerCore.Data.EntityFramework.Abstractions;

namespace CourseAI.Domain.Seeders;

public class RolesSeeder : IEntityDataSeeder<Role>
{
    public IEnumerable<Role> Data => new Role[]
    {
        new()
        {
            Id = 1L,
            Name = "driver",
            NormalizedName = "DRIVER",
        },
        new()
        {
            Id = 2L,
            Name = "orgadmin",
            NormalizedName = "ORGADMIN"
        },
        new()
        {
            Id = 3L,
            Name = "admin",
            NormalizedName = "ADMIN"
        }
    };
}