using Fleet.Core.Constants;
using Fleet.Domain.Entities;
using FluentValidation;

namespace Fleet.Application.Models.UserRoadmaps;

public abstract class UserRoadmapModelBase
{
    public long UserId { get; init; }
    public Guid RoadmapId { get; init; }

    /// <summary>
    /// Configures base validation rules.
    /// </summary>
    protected static void ConfigureBaseValidator<TModel>(InlineValidator<TModel> validator)
        where TModel : UserRoadmapModelBase
    {
        validator.RuleFor(x => x.UserId).NotEmpty().WithMessage("User ID is required.");
        validator.RuleFor(x => x.RoadmapId).NotEmpty().WithMessage("Roadmap ID is required.");
    }

    public UserRoadmap ToEntity() =>
        new()
        {
            UserId = UserId,
            RoadmapId = RoadmapId
        };
}