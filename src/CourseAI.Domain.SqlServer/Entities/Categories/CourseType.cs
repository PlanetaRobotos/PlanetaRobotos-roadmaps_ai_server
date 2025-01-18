using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NeerCore.Data.Abstractions;

namespace CourseAI.Domain.Entities.Categories;

public sealed class CourseType : IDateableEntity<Guid>
{
    public Guid Id { get; init; }
    public DateTime Created { get; init; } = DateTime.UtcNow;
    public DateTime? Updated { get; init; }
    
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public int Order { get; set; }
    
    // Navigation property
    public ICollection<CourseTypeRelation> CourseTypes { get; set; } = new List<CourseTypeRelation>();
    
    internal sealed class CourseTypeConfiguration : IEntityTypeConfiguration<CourseType>
    {
        public void Configure(EntityTypeBuilder<CourseType> builder)
        {
            builder.HasKey(t => t.Id);
            builder.Property(e => e.Id).HasDefaultValueSql("NEWID()");

            builder.Property(e => e.Created).IsRequired();
            builder.Property(e => e.Updated).IsRequired(false);
        
            builder.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(50);
            
            builder.Property(t => t.Description)
                .HasMaxLength(500);
            
            builder.Property(t => t.Order)
                .IsRequired();
            
            builder.HasIndex(t => t.Name).IsUnique();
            builder.HasIndex(t => t.Order);
        }
    }
}