using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NeerCore.Data.Abstractions;

namespace Fleet.Domain.Entities.Identity;

public class RoleClaim : IdentityRoleClaim<long>, IEntity
{
    internal class Configuration : IEntityTypeConfiguration<RoleClaim>
    {
        public void Configure(EntityTypeBuilder<RoleClaim> builder)
        {
            builder.ToTable($"{nameof(RoleClaim)}s");
        }
    }
}