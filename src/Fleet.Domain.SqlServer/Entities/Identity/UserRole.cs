using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NeerCore.Data.Abstractions;

namespace Fleet.Domain.Entities.Identity;

public class UserRole : IdentityUserRole<long>, IEntity
{
    internal class Configuration : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.ToTable($"{nameof(UserRole)}s");
        }
    }
}