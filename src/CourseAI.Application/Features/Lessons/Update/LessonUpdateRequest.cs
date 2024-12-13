using System.Text.Json.Serialization;
using CourseAI.Application.Core;
using CourseAI.Application.Models.Roadmaps;
using FluentValidation;

namespace CourseAI.Application.Features.Lessons.Update;

public class LessonUpdateRequest
    : LessonModel, IValidatable<LessonUpdateRequest>, IRequestModel
{
    [JsonIgnore]
    public Guid Id { get; set; }

    public Guid? LessonId { get; set; } // The ID of the step to update
    public bool? LessonCompleted { get; set; } // The new status of the step

    public void ConfigureValidator(InlineValidator<LessonUpdateRequest> validator)
    {
        validator.RuleFor(x => x.LessonId)
            .NotEmpty()
            .When(x => x.LessonCompleted.HasValue)
            .WithMessage("StepId is required when updating step status.");
    }
}
