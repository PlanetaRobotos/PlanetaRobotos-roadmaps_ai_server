using Fleet.Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fleet.Domain.Entities;

public class UserRoadmap
{
    public long UserId { get; init; }
    public Guid RoadmapId { get; init; }
    
    public User User { get; init; }
    public Roadmap Roadmap { get; init; }

    internal class Configuration : IEntityTypeConfiguration<UserRoadmap>
    {
        public void Configure(EntityTypeBuilder<UserRoadmap> builder)
        {
            builder.HasKey(e => new { e.UserId, e.RoadmapId });

            builder.Property(e => e.UserId).ValueGeneratedNever(); // No default value
            builder.Property(e => e.RoadmapId).HasDefaultValueSql("NEWID()");
            
            // Relationship with User
            builder.HasOne(e => e.User)
                .WithMany(u => u.UserRoadmaps)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relationship with Roadmap
            builder.HasOne(e => e.Roadmap)
                .WithMany(r => r.UserRoadmaps)
                .HasForeignKey(e => e.RoadmapId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
