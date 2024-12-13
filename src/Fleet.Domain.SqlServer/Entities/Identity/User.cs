using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NeerCore.Data.Abstractions;

namespace Fleet.Domain.Entities.Identity;

public class User : IdentityUser<long>, IDateableEntity<long>
{
    public DateTime Created { get; init; }
    public DateTime? Updated { get; init; }

    public ICollection<UserTableSettings>? TableSettings { get; init; }
    public ICollection<UserRoadmap>? UserRoadmaps { get; init; }

    internal class Configuration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable($"{nameof(User)}s");

            builder.HasMany(x => x.TableSettings).WithOne(x => x.User).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(x => x.UserRoadmaps).WithOne(x => x.User).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}