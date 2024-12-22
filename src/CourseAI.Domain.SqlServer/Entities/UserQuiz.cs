using CourseAI.Domain.Entities.Identity;
using CourseAI.Domain.Entities.Roadmaps;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NeerCore.Data.Abstractions;

namespace CourseAI.Domain.Entities;

public class UserQuiz : IEntity
{
    public long UserId { get; init; }
    public Guid QuizId { get; init; }
    public int AnswerIndex { get; init; }
    
    public User? User { get; init; }
    public Quiz? Quiz { get; init; }

    internal class Configuration : IEntityTypeConfiguration<UserQuiz>
    {
        public void Configure(EntityTypeBuilder<UserQuiz> builder)
        {
            builder.ToTable($"{nameof(UserQuiz)}es").HasKey(e => new { e.UserId, e.QuizId });
            
            // Relationship with User
            builder.HasOne(e => e.User)
                .WithMany(u => u.UserQuizzes)
                .HasForeignKey(e => e.UserId);

            // Relationship with Quiz
            builder.HasOne(e => e.Quiz)
                .WithMany(q => q.UserQuizzes)
                .HasForeignKey(e => e.QuizId);
        }
    }
}
