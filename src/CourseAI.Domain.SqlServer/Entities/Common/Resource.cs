using CourseAI.Core.Constants;
using CourseAI.Core.Enums;
using CourseAI.Core.Enums.Routes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NeerCore.Data.Abstractions;

namespace CourseAI.Domain.Entities.Common;

public class Resource : IDateableEntity<Guid>
{
    public Guid Id { get; init; }

    public string Path { get; set; } = null!;
    public string MediaType { get; set; } = null!;
    public ResourceType Type { get; set; }
    public ResourceStorage Storage { get; set; }

    public DateTime Created { get; init; }
    public DateTime? Updated { get; init; }


    internal class Configuration : IEntityTypeConfiguration<Resource>
    {
        public void Configure(EntityTypeBuilder<Resource> builder)
        {
            builder.ToTable($"{nameof(Resource)}s").HasKey(e => e.Id);

            builder.Property(e => e.Path).HasMaxLength(StringLimits._500).IsRequired();
            builder.Property(e => e.MediaType).HasMaxLength(StringLimits._100).IsRequired();
            builder.Property(e => e.Type).HasMaxLength(StringLimits._50).IsRequired();
            builder.Property(e => e.Storage).HasMaxLength(StringLimits._50).IsRequired();
        }
    }
}