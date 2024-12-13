using CourseAI.Application.Core;
using CourseAI.Application.Models.Roadmaps;
using FluentValidation;

namespace CourseAI.Application.Features.Roadmaps.Create;

public class RoadmapCreateRequest
    : RoadmapModelBase, IValidatable<RoadmapCreateRequest>, IRequestModel<RoadmapModel>
{
    public void ConfigureValidator(InlineValidator<RoadmapCreateRequest> validator)
    {
        ConfigureBaseValidator(validator);

        validator.RuleFor(x => x.Title).NotEmpty().WithMessage("Title is required.");
        // validator.RuleFor(x => x.Topic).NotEmpty().WithMessage("Topic is required.");
        // validator.RuleFor(x => x.Modules).NotNull().WithMessage("Modules cannot be null.");
    }
}
