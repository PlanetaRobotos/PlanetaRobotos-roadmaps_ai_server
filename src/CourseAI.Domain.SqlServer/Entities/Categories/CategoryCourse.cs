using CourseAI.Core.Types;
using CourseAI.Domain.Entities.Roadmaps;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NeerCore.Data.Abstractions;

namespace CourseAI.Domain.Entities.Categories;

public sealed class CategoryCourse : IDateableEntity<Guid>, IOrderable
{
    public Guid Id { get; init; }
    public DateTime Created { get; init; } = DateTime.UtcNow;
    public DateTime? Updated { get; init; }
    
    public Guid CategoryId { get; set; }
    public Guid RoadmapId { get; set; }  // Maps to Roadmap.Id
    public int Order { get; set; }
    
    // Navigation properties
    public Category Category { get; set; } = null!;
    public Roadmap Roadmap { get; set; } = null!;

    internal sealed class CategoryCourseConfiguration : IEntityTypeConfiguration<CategoryCourse>
    {
        public void Configure(EntityTypeBuilder<CategoryCourse> builder)
        {
            builder.HasKey(cc => cc.Id);
            builder.Property(e => e.Id).HasDefaultValueSql("NEWID()");

            builder.Property(e => e.Created).IsRequired();
            builder.Property(e => e.Updated).IsRequired(false);
        
            builder.Property(cc => cc.Order)
                .IsRequired();
            
            builder.HasOne(cc => cc.Category)
                .WithMany(c => c.CategoryCourses)
                .HasForeignKey(cc => cc.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);
            
            builder.HasOne(cc => cc.Roadmap)
                .WithMany(r => r.CategoryCourses)
                .HasForeignKey(cc => cc.RoadmapId)
                .OnDelete(DeleteBehavior.Cascade);
            
            builder.HasIndex(cc => new { cc.CategoryId, cc.RoadmapId }).IsUnique();
            builder.HasIndex(cc => new { cc.CategoryId, cc.Order });
        }
    }
}