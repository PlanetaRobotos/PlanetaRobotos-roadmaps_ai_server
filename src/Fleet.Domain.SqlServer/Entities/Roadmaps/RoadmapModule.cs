using Fleet.Core.Constants;
using Fleet.Domain.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NeerCore.Data.Abstractions;

namespace Fleet.Domain.Entities.Roadmaps;

public class RoadmapModule: IDateableEntity<Guid>
{
    public Guid Id { get; init; }
    public string Title { get; init; } = null!;
    public int Order { get; init; }
    public Guid RoadmapId { get; init; }
    public Roadmap Roadmap { get; init; } = null!;

    public ICollection<Lesson>? Lessons { get; init; }
    
    public DateTime? Updated { get; }
    public DateTime Created { get; }
    
    internal class Configuration : IEntityTypeConfiguration<RoadmapModule>
    {
        public void Configure(EntityTypeBuilder<RoadmapModule> builder)
        {
            builder.ToTable($"{nameof(RoadmapModule)}s").HasKey(e => e.Id);

            builder.Property(e => e.Id).HasSequentialGuidAsDefault();
            builder.Property(e => e.Title).HasMaxLength(StringLimits._255);
            builder.Property(e => e.Order).IsRequired();
            builder.HasMany(e => e.Lessons).WithOne(e => e.RoadmapModule).HasForeignKey(e => e.ModuleId);
        }
    }
}