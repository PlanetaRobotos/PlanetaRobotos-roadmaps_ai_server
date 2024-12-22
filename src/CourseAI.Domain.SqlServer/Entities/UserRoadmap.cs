using CourseAI.Domain.Entities.Identity;
using CourseAI.Domain.Entities.Roadmaps;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NeerCore.Data.Abstractions;

namespace CourseAI.Domain.Entities;

public class UserRoadmap : IEntity
{
    public long UserId { get; init; }
    public Guid RoadmapId { get; init; }
    public bool IsLiked { get; set; }
    
    public User? User { get; init; }
    public Roadmap? Roadmap { get; init; }
    
    // public ICollection<UserQuiz> UserQuizzes { get; set; }

    internal class Configuration : IEntityTypeConfiguration<UserRoadmap>
    {
        public void Configure(EntityTypeBuilder<UserRoadmap> builder)
        {
            builder.ToTable($"{nameof(UserRoadmap)}s").HasKey(e => new { e.UserId, e.RoadmapId });

            builder.HasOne(e => e.User)
                .WithMany(u => u.UserRoadmaps)
                .HasForeignKey(e => e.UserId);

            builder.HasOne(e => e.Roadmap)
                .WithMany(r => r.UserRoadmaps)
                .HasForeignKey(e => e.RoadmapId);
        }
    }
}