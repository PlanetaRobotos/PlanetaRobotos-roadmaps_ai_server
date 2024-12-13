using CourseAI.Application.Core;
using CourseAI.Application.Models.Roadmaps;
using FluentValidation;

namespace CourseAI.Application.Features.Lessons.Create;

public class LessonCreateRequest
    : LessonModel, IValidatable<LessonCreateRequest>, IRequestModel<LessonModel>
{
    public void ConfigureValidator(InlineValidator<LessonCreateRequest> validator)
    {
        validator.RuleFor(x => x.Title).NotEmpty().WithMessage("Title is required.");
    }
}
