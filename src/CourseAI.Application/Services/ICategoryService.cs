using CourseAI.Application.Models.Categories;
using CourseAI.Application.Models.Roadmaps;

namespace CourseAI.Application.Services;

public interface ICategoryService
{
    Task<List<CategoryModel>> GetCategoriesAsync(Guid? parentId);
    Task<CategoryPageModel> GetCategoryPageAsync(Guid id);
    Task<List<RoadmapModel>> GetCategoryCoursesAsync(Guid id, int skip, int take, string sortBy);
}