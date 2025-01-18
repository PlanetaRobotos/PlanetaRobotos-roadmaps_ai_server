using CourseAI.Api.Core;
using CourseAI.Application.Models.Categories;
using CourseAI.Application.Models.Roadmaps;
using CourseAI.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace CourseAI.Api.Controllers;

public class ExploreController : V1Controller
{
    private readonly IExploreService _exploreService;

    public ExploreController(IExploreService exploreService) =>
        _exploreService = exploreService;

    [HttpGet("page")]
    [ProducesResponseType<ExplorePageModel>(StatusCodes.Status200OK)]
    public Task<ExplorePageModel> GetExplorePage()
    {
        return _exploreService.GetExplorePageAsync();
    }

    [HttpGet("new")]
    public Task<List<RoadmapModel>> GetNewCourses(
        [FromQuery] int skip,
        [FromQuery] int take)
    {
        return _exploreService.GetNewCoursesAsync(skip, take);
    }

    [HttpGet("top")]
    public Task<List<RoadmapModel>> GetTopCourses(
        [FromQuery] int skip,
        [FromQuery] int take)
    {
        return _exploreService.GetTopCoursesAsync(skip, take);
    }

    [HttpGet("better-you")]
    public Task<List<RoadmapModel>> GetBetterYouCourses(
        [FromQuery] int skip,
        [FromQuery] int take)
    {
        return _exploreService.GetBetterYouCoursesAsync(skip, take);
    }
}