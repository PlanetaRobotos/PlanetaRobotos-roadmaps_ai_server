using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NeerCore.Data.Abstractions;

namespace CourseAI.Domain.Entities.Categories;

public sealed class CategoryRelation : IDateableEntity<Guid>
{
    public Guid Id { get; init; }
    public DateTime Created { get; init; } = DateTime.UtcNow;
    public DateTime? Updated { get; init; }

    public Guid ParentCategoryId { get; set; }
    public Guid ChildCategoryId { get; set; }
    // public int Order { get; set; }

    // Navigation properties
    public Category ParentCategory { get; set; } = null!;
    public Category ChildCategory { get; set; } = null!;

    public sealed class CategoryRelationConfiguration : IEntityTypeConfiguration<CategoryRelation>
    {
        public void Configure(EntityTypeBuilder<CategoryRelation> builder)
        {
            builder.HasKey(cr => cr.Id);
            builder.Property(e => e.Id).HasDefaultValueSql("NEWID()");

            builder.Property(e => e.Created).IsRequired();
            builder.Property(e => e.Updated).IsRequired(false);

            // builder.Property(cr => cr.Order)
                // .IsRequired();

            // Relationships with restricted delete behavior to prevent cycles
            builder.HasOne(cr => cr.ParentCategory)
                .WithMany(c => c.ChildRelations)
                .HasForeignKey(cr => cr.ParentCategoryId)
                .OnDelete(DeleteBehavior.Restrict); // Changed from Cascade

            builder.HasOne(cr => cr.ChildCategory)
                .WithMany(c => c.ParentRelations)
                .HasForeignKey(cr => cr.ChildCategoryId)
                .OnDelete(DeleteBehavior.Restrict); // Changed from Cascade

            // Unique constraint and indexes
            builder.HasIndex(cr => new { cr.ParentCategoryId, cr.ChildCategoryId }).IsUnique();
            // builder.HasIndex(cr => new { cr.ParentCategoryId, cr.Order });
        }
    }
}