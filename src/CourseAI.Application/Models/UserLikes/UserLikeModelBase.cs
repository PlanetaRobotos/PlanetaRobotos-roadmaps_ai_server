using CourseAI.Application.Models.Roadmaps;
using CourseAI.Application.Models.UserRoadmaps;
using CourseAI.Domain.Entities;
using CourseAI.Domain.Entities.Roadmaps;
using FluentValidation;

namespace CourseAI.Application.Models.UserLikes;

public abstract class UserLikeModelBase
{
    public long UserId { get; init; }
    public Guid RoadmapId { get; init; }
    
    public UserModel? User { get; init; }
    public RoadmapModel? Roadmap { get; init; }

    /// <summary>
    /// Configures base validation rules.
    /// </summary>
    protected static void ConfigureBaseValidator<TModel>(InlineValidator<TModel> validator)
        where TModel : UserLikeModelBase
    {
        validator.RuleFor(x => x.UserId).NotEmpty().WithMessage("User ID is required.");
        validator.RuleFor(x => x.RoadmapId).NotEmpty().WithMessage("Roadmap ID is required.");
    }

    public UserLike ToEntity() =>
        new()
        {
            UserId = UserId,
            RoadmapId = RoadmapId,
            User = User?.ToEntity(),
            Roadmap = Roadmap?.ToEntity()
        };
}