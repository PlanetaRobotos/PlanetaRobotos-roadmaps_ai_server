using CourseAI.Core.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NeerCore.Data.Abstractions;
using Sieve.Services;

namespace CourseAI.Domain.Entities.Roadmaps;

/// <summary>
/// Represents a personalized learning roadmap entity.
/// </summary>

public class Roadmap: IDateableEntity<Guid>
{
    public Guid Id { get; init; }
    public string Title { get; set; } = null!;
    public string? Topic { get; set; }
    public int EstimatedDuration { get; set; }
    public string? Description { get; set; }
    public string[]? Tags { get; set; }
    public int? Likes { get; set; }

    public ICollection<RoadmapModule>? Modules { get; init; }

    public ICollection<UserRoadmap>? UserRoadmaps { get; init; }
    public ICollection<UserLike>? UserLikes { get; init; }
    
    public DateTime Created { get; init; } = DateTime.UtcNow;
    public DateTime? Updated { get; init; }

    internal class Configuration : IEntityTypeConfiguration<Roadmap>, ISieveConfiguration
    {
        public void Configure(EntityTypeBuilder<Roadmap> builder)
        {
            builder.ToTable($"{nameof(Roadmap)}s").HasKey(e => e.Id);
            builder.HasIndex(e => e.Title);

            builder.Property(e => e.Id).HasDefaultValueSql("NEWID()");
            builder.Property(e => e.Title).HasMaxLength(255).IsRequired();
            builder.Property(e => e.Description).HasMaxLength(StringLimits._1000);
            builder.Property(e => e.EstimatedDuration).IsRequired();
            builder.Property(e => e.Created).IsRequired();
            builder.Property(e => e.Updated).IsRequired(false);
            builder.Property(e => e.Likes).HasDefaultValue(0);

            builder.HasMany(e => e.Modules).WithOne(e => e.Roadmap).HasForeignKey(e => e.RoadmapId);
        }

        public void Configure(SievePropertyMapper mapper)
        {
            mapper.Property<Roadmap>(e => e.Title).CanFilter().CanSort();
            mapper.Property<Roadmap>(e => e.Created).CanFilter().CanSort();
            mapper.Property<Roadmap>(e => e.Updated).CanFilter().CanSort();
        }
    }
}