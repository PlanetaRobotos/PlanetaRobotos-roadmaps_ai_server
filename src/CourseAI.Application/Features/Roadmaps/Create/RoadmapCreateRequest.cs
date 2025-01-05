using CourseAI.Application.Core;
using CourseAI.Application.Models.Roadmaps;
using FluentValidation;

namespace CourseAI.Application.Features.Roadmaps.Create;

public class RoadmapCreateRequest
    : RoadmapModelBase, IValidatable<RoadmapCreateRequest>, IRequestModel<RoadmapModel>
{
    public int Price { get; set; }
    
    public void ConfigureValidator(InlineValidator<RoadmapCreateRequest> validator)
    {
        ConfigureBaseValidator(validator);

        validator.RuleFor(x => x.Title).NotEmpty().WithMessage("Title is required.");
        validator.RuleFor(x => x.Price).NotEmpty().GreaterThan(0).WithMessage("Price must be greater than 0.");
    }
}
