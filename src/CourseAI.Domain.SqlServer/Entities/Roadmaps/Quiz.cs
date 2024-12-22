using CourseAI.Domain.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NeerCore.Data.Abstractions;

namespace CourseAI.Domain.Entities.Roadmaps;

public class Quiz : IDateableEntity<Guid>
{
    public Guid Id { get; init; }
    public string Question { get; set; }
    public List<string> Answers { get; set; }
    public int CorrectAnswerIndex { get; set; }

    public Lesson Lesson { get; set; } = null!;
    public Guid LessonId { get; set; }
    
    public ICollection<UserQuiz> UserQuizzes { get; init; }

    public DateTime? Updated { get; }
    public DateTime Created { get; }

    internal class Configuration : IEntityTypeConfiguration<Quiz>
    {
        public void Configure(EntityTypeBuilder<Quiz> builder)
        {
            builder.ToTable($"{nameof(Quiz)}zes").HasKey(e => e.Id);
            builder.Property(e => e.Id).HasSequentialGuidAsDefault();

            builder.Property(e => e.Question).HasMaxLength(500).IsRequired();
            builder.Property(e => e.Answers).IsRequired();
            builder.Property(e => e.CorrectAnswerIndex).IsRequired();
        }
    }
}
