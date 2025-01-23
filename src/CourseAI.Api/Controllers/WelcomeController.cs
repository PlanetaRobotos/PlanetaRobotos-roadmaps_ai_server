using CourseAI.Api.Core;
using CourseAI.Application.Models.Categories;
using CourseAI.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace CourseAI.Api.Controllers;

public class WelcomeController(IExploreService exploreService) : V1Controller
{
    [HttpGet("page")]
    [ProducesResponseType<WelcomePageModel>(StatusCodes.Status200OK)]
    public async Task<WelcomePageModel> WelcomeCourses(
        [FromQuery] int skip,
        [FromQuery] int take)
    {
        var pageModel = new WelcomePageModel
        {
            WelcomeCourses = await exploreService.GetWelcomeCoursesAsync(skip, take)
        };
        
        return pageModel;
    }
}