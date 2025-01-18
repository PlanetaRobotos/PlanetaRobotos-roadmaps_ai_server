using CourseAI.Core.Constants;
using CourseAI.Core.Security;
using CourseAI.Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CourseAI.Domain.Entities;

public class UserPurchase
{
    public Guid Id { get; set; }
    public DateTime CreatedOnUtc { get; set; }
    public string OrderReference { get; set; } = string.Empty;

    public Roles Role { get; set; }
    public string? ActiveEmail { get; set; }
    public bool IsActivated { get; set; }

    internal sealed class UserPurchaseConfiguration : IEntityTypeConfiguration<UserPurchase>
    {
        public void Configure(EntityTypeBuilder<UserPurchase> builder)
        {
            builder.ToTable($"{nameof(UserPurchase)}s").HasKey(x => x.Id);
            builder.Property(x => x.OrderReference).HasMaxLength(255).IsRequired();
            builder.Property(e => e.Role).HasMaxLength(StringLimits._50).IsRequired();
        }
    }
}