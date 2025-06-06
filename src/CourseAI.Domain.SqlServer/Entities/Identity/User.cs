using Azure.Core.Pipeline;
using CourseAI.Domain.Entities.Roadmaps;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NeerCore.Data.Abstractions;

namespace CourseAI.Domain.Entities.Identity;

public class User : IdentityUser<long>, IDateableEntity<long>
{
    public DateTime Created { get; init; }
    public DateTime? Updated { get; init; }

    public string? Name { get; set; }
    public int Tokens { get; set; }
    public ICollection<UserTableSettings>? TableSettings { get; init; }
    public ICollection<UserRoadmap>? UserRoadmaps { get; init; }
    public ICollection<UserLike>? UserLikes { get; init; }
    public ICollection<UserQuiz> UserQuizzes { get; init; }
    public string? Bio { get; set; }
    
    public Guid UserRoadmapId { get; set; }

    internal class Configuration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable($"{nameof(User)}s");

            builder.HasMany(x => x.TableSettings)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            
            // limit length of Name to 100 characters
            builder.Property(x => x.Name)
                .HasMaxLength(100);
            
            builder.Property(x => x.Bio)
                .HasMaxLength(500)  // or any other reasonable limit
                .IsRequired(false);
        }
    }
}
