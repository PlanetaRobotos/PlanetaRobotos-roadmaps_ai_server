using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NeerCore.Data.Abstractions;

namespace CourseAI.Domain.Entities.Categories;

public sealed class Category : IDateableEntity<Guid>
{
    public Guid Id { get; init; }
    public DateTime Created { get; init; } = DateTime.UtcNow;
    public DateTime? Updated { get; init; }
    
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public string ColorHex { get; set; } = null!;
    public string? ThumbnailUrl { get; set; }
    public int Order { get; set; }
    public int? Position { get; set; }
    
    // Navigation properties
    public ICollection<CategoryCourse> CategoryCourses { get; set; } = new List<CategoryCourse>();
    public ICollection<CategoryRelation> ParentRelations { get; set; } = new List<CategoryRelation>();
    public ICollection<CategoryRelation> ChildRelations { get; set; } = new List<CategoryRelation>();

    private class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(e => e.Id).HasDefaultValueSql("NEWID()");
            
            builder.Property(e => e.Created).IsRequired();
            builder.Property(e => e.Updated).IsRequired(false);
        
            builder.Property(c => c.Title)
                .IsRequired()
                .HasMaxLength(100);
            
            builder.Property(c => c.Description)
                .HasMaxLength(500);
            
            builder.Property(c => c.ColorHex)
                .IsRequired()
                .HasMaxLength(7);
            
            builder.Property(c => c.Order)
                .IsRequired();
            
            builder.HasIndex(c => c.Order);
        }
    }
}