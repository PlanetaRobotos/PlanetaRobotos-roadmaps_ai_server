using System.Text.Json.Serialization;
using CourseAI.Application.Core;
using CourseAI.Application.Models.Roadmaps;
using FluentValidation;

namespace CourseAI.Application.Features.Roadmaps.Update;

public class RoadmapUpdateRequest
    : RoadmapModelBase, IValidatable<RoadmapUpdateRequest>, IRequestModel
{
    [JsonIgnore]
    public Guid Id { get; set; }

    public Guid? LessonId { get; set; }
    public string? LessonContent { get; set; }
    
    // public Guid? QuizId { get; set; }
    // public string? Question { get; set; }
    // public List<string>? Answers { get; set; }

    public void ConfigureValidator(InlineValidator<RoadmapUpdateRequest> validator)
    {
        ConfigureBaseValidator(validator);

        // validator.RuleFor(x => x.LessonId)
        //     .NotEmpty()
        //     .When(x => x.LessonCompleted.HasValue)
        //     .WithMessage("StepId is required when updating step status.");

        validator.RuleFor(x => x.EstimatedDuration)
            .GreaterThan(0)
            .When(x => x.EstimatedDuration.HasValue)
            .WithMessage("Estimated duration must be greater than zero.");
    }
}
