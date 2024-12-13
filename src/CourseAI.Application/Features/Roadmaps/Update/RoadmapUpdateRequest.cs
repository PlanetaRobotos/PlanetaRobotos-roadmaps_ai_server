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

    public Guid? LessonId { get; set; } // The ID of the step to update
    public bool? LessonCompleted { get; set; } // The new status of the step

    public void ConfigureValidator(InlineValidator<RoadmapUpdateRequest> validator)
    {
        ConfigureBaseValidator(validator);

        validator.RuleFor(x => x.LessonId)
            .NotEmpty()
            .When(x => x.LessonCompleted.HasValue)
            .WithMessage("StepId is required when updating step status.");

        validator.RuleFor(x => x.EstimatedDuration)
            .GreaterThan(0)
            .When(x => x.EstimatedDuration.HasValue)
            .WithMessage("Estimated duration must be greater than zero.");
    }
}
