using CourseAI.Application.Models.Roadmaps;

namespace CourseAI.Application.Models.Categories;

public sealed record CategoryModel
{
    public Guid Id { get; init; }
    public string Title { get; init; } = null!;
    public string? Description { get; init; }
    public string ColorHex { get; init; } = null!;
    public string? ThumbnailUrl { get; init; } = null!; 
    public int? Position { get; init; }

    // Navigation properties that match our entity structure
    public List<RoadmapModel> Courses { get; init; } = new();
    public List<CategoryModel> ParentRelations { get; init; } = new();
    public List<CategoryModel> ChildCategories { get; init; } = new();
}

public sealed record CategoryPageModel
{
    public CategoryModel Category { get; init; } = null!;
    public List<RoadmapModel> TopCourses { get; init; } = new();
    public List<RoadmapModel> NewCourses { get; init; } = new();
    public List<CategorySliderModel> RelatedCategories { get; init; } = new();
    public List<CategorySliderModel> ChildCategories { get; set; } = new(); // Add this
}

public sealed record CategorySliderModel
{
    public Guid Id { get; init; }
    public string Title { get; init; } = null!;
    public string ColorHex { get; init; } = null!;
    public int Order { get; init; }
}

public sealed record CourseTypeModel
{
    public Guid Id { get; init; }
    public string Name { get; init; } = null!;
    public string? Description { get; init; }
    public int Order { get; init; }
}

public sealed record CategoryCoursesModel
{
    public Guid CategoryId { get; init; }
    public List<RoadmapModel> Courses { get; init; } = new();
    public int TotalCount { get; init; }
}

// Request/Filter models
public sealed record CategoryCoursesRequest
{
    public Guid CategoryId { get; init; }
    public int Skip { get; init; }
    public int Take { get; init; }
    public string SortBy { get; init; } = "new"; // new, top, essential
}

public sealed record CoursesByTypeRequest
{
    public string TypeName { get; init; } = null!;
    public int Skip { get; init; }
    public int Take { get; init; }
}

// Admin/Management models
public sealed record CategoryCreateModel
{
    public string Title { get; init; } = null!;
    public string? Description { get; init; }
    public string ColorHex { get; init; } = null!;
    public int Order { get; init; }
    public Guid? ParentCategoryId { get; init; }
}

public sealed record CategoryUpdateModel
{
    public Guid Id { get; init; }
    public string Title { get; init; } = null!;
    public string? Description { get; init; }
    public string ColorHex { get; init; } = null!;
    public int Order { get; init; }
}

public sealed record CourseTypeCreateModel
{
    public string Name { get; init; } = null!;
    public string? Description { get; init; }
    public int Order { get; init; }
}

public sealed record CourseTypeUpdateModel
{
    public string Name { get; init; } = null!;
    public string? Description { get; init; }
    public int Order { get; init; }
}

public sealed record ExplorePageModel
{
    public List<CategorySliderModel> Categories { get; init; } = new();
    public List<RoadmapModel> NewCourses { get; init; } = new();
    public List<RoadmapModel> TopCourses { get; init; } = new();
    public List<RoadmapModel> BetterYouCourses { get; init; } = new();
    public List<RoadmapModel> NewToLevenueCourses { get; init; } = new();
}

public sealed record WelcomePageModel
{
    public List<RoadmapModel> WelcomeCourses { get; init; } = new();
}

public sealed record PositionRequest
{
    public int? Position { get; init; }
}

public sealed record AddCategoryRelationRequest
{
    public int Order { get; init; }
}