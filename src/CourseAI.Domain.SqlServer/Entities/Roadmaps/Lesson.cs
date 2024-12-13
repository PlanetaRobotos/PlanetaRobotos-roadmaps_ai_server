using System.Text.Json;
using CourseAI.Core.Constants;
using CourseAI.Domain.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NeerCore.Data.Abstractions;

namespace CourseAI.Domain.Entities.Roadmaps;

public class Lesson : IDateableEntity<Guid>
{
    public Guid Id { get; init; }
    public string Title { get; init; } = null!;
    public bool Completed { get; set; } = false;
    public int Order { get; init; }
    public Guid ModuleId { get; init; }
    public RoadmapModule RoadmapModule { get; init; } = null!;
    
    public string? Description { get; set; }
    public LessonContent? Content { get; set; }
    public DateTime? Updated { get; }
    public DateTime Created { get; }
    
    internal class Configuration : IEntityTypeConfiguration<Lesson>
    {
        public void Configure(EntityTypeBuilder<Lesson> builder)
        {
            builder.ToTable($"{nameof(Lesson)}s").HasKey(e => e.Id);

            builder.Property(e => e.Id).HasSequentialGuidAsDefault();
            builder.Property(e => e.Title).HasMaxLength(StringLimits._255);
            builder.Property(e => e.Order).IsRequired();
            
            builder.Property(e => e.Description).HasMaxLength(StringLimits._1000);
            builder.Property(e => e.Content)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, new JsonSerializerOptions { WriteIndented = false }),
                    v => JsonSerializer.Deserialize<LessonContent>(v, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                )
                .HasColumnType("nvarchar(max)");
            
            builder.HasOne(e => e.RoadmapModule)
                .WithMany(m => m.Lessons)
                .HasForeignKey(e => e.ModuleId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
