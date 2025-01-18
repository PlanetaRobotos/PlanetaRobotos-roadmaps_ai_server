using CourseAI.Domain.Entities.Roadmaps;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NeerCore.Data.Abstractions;

namespace CourseAI.Domain.Entities.Categories;

public sealed class CourseTypeRelation : IDateableEntity<Guid>
{
    public Guid Id { get; init; }
    public DateTime Created { get; init; } = DateTime.UtcNow;
    public DateTime? Updated { get; init; }
    
    public Guid RoadmapId { get; set; }  // Maps to Roadmap.Id
    public Guid TypeId { get; set; }
    
    // Navigation properties
    public Roadmap Roadmap { get; set; } = null!;
    public CourseType Type { get; set; } = null!;
    
    internal sealed class CourseTypeRelationConfiguration : IEntityTypeConfiguration<CourseTypeRelation>
    {
        public void Configure(EntityTypeBuilder<CourseTypeRelation> builder)
        {
            builder.HasKey(ct => ct.Id);
            builder.Property(e => e.Id).HasDefaultValueSql("NEWID()");

            builder.Property(e => e.Created).IsRequired();
            builder.Property(e => e.Updated).IsRequired(false);
        
            builder.HasOne(ct => ct.Roadmap)
                .WithMany(r => r.CourseTypes)
                .HasForeignKey(ct => ct.RoadmapId)
                .OnDelete(DeleteBehavior.Cascade);
            
            builder.HasOne(ct => ct.Type)
                .WithMany(t => t.CourseTypes)
                .HasForeignKey(ct => ct.TypeId)
                .OnDelete(DeleteBehavior.Cascade);
            
            builder.HasIndex(ct => new { ct.RoadmapId, ct.TypeId }).IsUnique();
        }
    }

}