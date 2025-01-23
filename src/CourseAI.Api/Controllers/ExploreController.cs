using CourseAI.Api.Core;
using CourseAI.Application.Models.Categories;
using CourseAI.Application.Models.Roadmaps;
using CourseAI.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace CourseAI.Api.Controllers;

public class ExploreController(IExploreService exploreService) : V1Controller
{
    [HttpGet("page")]
    [ProducesResponseType<ExplorePageModel>(StatusCodes.Status200OK)]
    public Task<ExplorePageModel> GetExplorePage()
    {
        return exploreService.GetExplorePageAsync();
    }

    [HttpGet("new")]
    public Task<List<RoadmapModel>> GetNewCourses(
        [FromQuery] int skip,
        [FromQuery] int take)
    {
        return exploreService.GetNewCoursesAsync(skip, take);
    }

    [HttpGet("top")]
    public Task<List<RoadmapModel>> GetTopCourses(
        [FromQuery] int skip,
        [FromQuery] int take)
    {
        return exploreService.GetTopCoursesAsync(skip, take);
    }

    [HttpGet("better-you")]
    public Task<List<RoadmapModel>> GetBetterYouCourses(
        [FromQuery] int skip,
        [FromQuery] int take)
    {
        return exploreService.GetBetterYouCoursesAsync(skip, take);
    }
}