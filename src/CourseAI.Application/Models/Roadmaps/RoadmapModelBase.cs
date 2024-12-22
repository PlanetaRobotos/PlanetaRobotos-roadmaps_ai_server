using CourseAI.Core.Constants;
using CourseAI.Domain.Entities;
using CourseAI.Domain.Entities.Roadmaps;
using FluentValidation;

namespace CourseAI.Application.Models.Roadmaps;

public abstract class RoadmapModelBase
{
    /// <example>Python Basics</example>
    public string? Title { get; set; }

    /// <example>Python</example>
    public string? Topic { get; set; }

    /// <example>10</example>
    public int? EstimatedDuration { get; set; }

    /// <example>This roadmap helps you get started with Python programming.</example>
    public string? Description { get; set; }

    /// <example>["coding", "python", "beginner"]</example>
    public string[]? Tags { get; set; }
    

    /// <example>0</example>
    public int? Likes { get; set; }

    public IList<RoadmapModuleModel> Modules { get; set; } = new List<RoadmapModuleModel>();

    /// <summary>
    /// Configures base validation rules.
    /// </summary>
    protected static void ConfigureBaseValidator<TModel>(InlineValidator<TModel> validator)
        where TModel : RoadmapModelBase
    {
        validator.RuleFor(x => x.EstimatedDuration).GreaterThan(0).WithMessage("Estimated duration must be greater than 0.");
        validator.RuleFor(x => x.Description).MaximumLength(StringLimits._1000).WithMessage("Description is too long.");
        validator.RuleFor(x => x.Likes).GreaterThanOrEqualTo(0).WithMessage("Likes must be greater than or equal to 0.");
    }

    public Roadmap ToEntity() =>
        new()
        {
            Id = Guid.NewGuid(),
            Title = Title ?? "Untitled",
            Topic = Topic ?? "General",
            EstimatedDuration = EstimatedDuration ?? 0,
            Description = Description,
            Tags = Tags,
            Likes = Likes,
            Created = DateTime.UtcNow,
            Modules = Modules.Select(module => module.ToEntity()).ToList()
        };
}
