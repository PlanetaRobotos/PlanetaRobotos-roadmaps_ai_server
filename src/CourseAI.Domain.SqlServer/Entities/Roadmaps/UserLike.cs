using CourseAI.Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NeerCore.Data.Abstractions;

namespace CourseAI.Domain.Entities.Roadmaps;

public class UserLike: IDateableEntity
{
    public long UserId { get; init; }
    public Guid RoadmapId { get; init; }

    public User? User { get; init; }
    public Roadmap? Roadmap { get; init; }
    
    public DateTime Created { get; }
    public DateTime? Updated { get; }
    
    internal class Configuration : IEntityTypeConfiguration<UserLike>
    {
        public void Configure(EntityTypeBuilder<UserLike> builder)
        {
            builder.ToTable($"{nameof(UserLike)}s").HasKey(e => new { e.UserId, e.RoadmapId });
            
            builder.Property(e => e.Created).IsRequired();
            builder.Property(e => e.Updated).IsRequired(false);
            
            builder.HasOne(e => e.User)
                .WithMany(u => u.UserLikes)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            
            builder.HasOne(e => e.Roadmap)
                .WithMany(r => r.UserLikes)
                .HasForeignKey(e => e.RoadmapId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}