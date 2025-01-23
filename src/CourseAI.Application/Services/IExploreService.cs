using CourseAI.Application.Models.Categories;
using CourseAI.Application.Models.Roadmaps;

namespace CourseAI.Application.Services;

public interface IExploreService
{
    Task<ExplorePageModel> GetExplorePageAsync();
    Task<List<RoadmapModel>> GetNewCoursesAsync(int skip, int take);
    Task<List<RoadmapModel>> GetTopCoursesAsync(int skip, int take);
    Task<List<RoadmapModel>> GetBetterYouCoursesAsync(int skip, int take);
    Task<List<RoadmapModel>> GetWelcomeCoursesAsync(int skip, int take);
    Task<List<CategoryModel>> GetCategoriesAsync();
}