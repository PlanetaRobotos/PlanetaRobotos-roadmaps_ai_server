using CourseAI.Core.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NeerCore.Data.Abstractions;

namespace CourseAI.Domain.Entities.Transactions;

public class TokenTransaction : IDateableEntity<Guid>
{
    public Guid Id { get; init; }
    public long UserId { get; init; }
    public int Amount { get; init; }
    public TransactionType TransactionType { get; init; }
    public DateTime Created { get; init; } = DateTime.UtcNow;
    public DateTime? Updated { get; init; }

    internal class Configuration : IEntityTypeConfiguration<TokenTransaction>
    {
        public void Configure(EntityTypeBuilder<TokenTransaction> builder)
        {
            builder.ToTable($"{nameof(TokenTransaction)}s").HasKey(e => e.Id);

            builder.Property(e => e.Id).HasDefaultValueSql("NEWID()");
            builder.Property(e => e.UserId).IsRequired();
            builder.Property(e => e.Amount).IsRequired();
            builder.Property(e => e.TransactionType).IsRequired();
            builder.Property(e => e.Created).IsRequired();
            builder.Property(e => e.Updated).IsRequired(false);
        }
    }
}