using CourseAI.Application.Models.Categories;
using CourseAI.Application.Models.Roadmaps;
using CourseAI.Application.Services;
using CourseAI.Domain.Context;
using Mapster;
using Microsoft.EntityFrameworkCore;
using NeerCore.DependencyInjection;

namespace CourseAI.Infrastructure.Services;

[Service]
public class ExploreService : IExploreService
{
    private readonly AppDbContext _context;
    private readonly IDbContextFactory<AppDbContext> _contextFactory;

    private const string BetterYouTypeName = "BetterYou2025";
    private const string WelcomeTypeName = "Welcome";
    private const string NewToLevenueTypeName = "NewToLevenue";

    public ExploreService(AppDbContext context, IDbContextFactory<AppDbContext> contextFactory)
    {
        _context = context;
        _contextFactory = contextFactory;
    }

    public async Task<ExplorePageModel> GetExplorePageAsync()
    {
        // Create separate contexts for parallel operations
        await using var categoriesContext = await _contextFactory.CreateDbContextAsync();
        await using var newContext = await _contextFactory.CreateDbContextAsync();
        await using var topContext = await _contextFactory.CreateDbContextAsync();
        await using var betterYouContext = await _contextFactory.CreateDbContextAsync();
        await using var newToLevenueContext = await _contextFactory.CreateDbContextAsync();

        // Get root categories
        var categoriesTask = categoriesContext.Categories
            .Where(c => !c.ParentRelations.Any()) // Root categories only
            .OrderBy(c => c.Position)
            .ProjectToType<CategorySliderModel>()
            .ToListAsync();

        // Get new courses
        var newCoursesTask = newContext.Roadmaps
            .OrderByDescending(r => r.Created)
            .Take(20) // Two rows of courses
            .ProjectToType<RoadmapModel>()
            .ToListAsync();

        // Get top courses
        var topCoursesTask = topContext.Roadmaps
            .OrderByDescending(r => r.Likes)
            .Take(10) // One row with indices
            .ProjectToType<RoadmapModel>()
            .ToListAsync();

        // Get "Better You" courses
        var betterYouCoursesTask = betterYouContext.Roadmaps
            .Where(r => r.CourseTypes
                .Any(rt => rt.Type.Name == BetterYouTypeName))
            .OrderByDescending(r => r.Created)
            .Take(10) // One row
            .ProjectToType<RoadmapModel>()
            .ToListAsync();
        
        // Get "New to Levenue" courses
        var newToLevenueCoursesTask = newToLevenueContext.Roadmaps
            .Where(r => r.CourseTypes
                .Any(rt => rt.Type.Name == NewToLevenueTypeName))
            .OrderByDescending(r => r.Created)
            .Take(10) // One row
            .ProjectToType<RoadmapModel>()
            .ToListAsync();

        // Wait for all parallel tasks to complete
        await Task.WhenAll(
            categoriesTask,
            newCoursesTask,
            topCoursesTask,
            betterYouCoursesTask,
            newToLevenueCoursesTask);

        return new ExplorePageModel
        {
            Categories = await categoriesTask,
            NewCourses = await newCoursesTask,
            TopCourses = await topCoursesTask,
            BetterYouCourses = await betterYouCoursesTask,
            NewToLevenueCourses = await newToLevenueCoursesTask
        };
    }

    public Task<List<RoadmapModel>> GetNewCoursesAsync(int skip, int take)
    {
        return _context.Roadmaps
            .OrderByDescending(r => r.Created)
            .Skip(skip)
            .Take(take)
            .ProjectToType<RoadmapModel>()
            .ToListAsync();
    }

    public Task<List<RoadmapModel>> GetTopCoursesAsync(int skip, int take)
    {
        return _context.Roadmaps
            .OrderByDescending(r => r.Likes)
            .Skip(skip)
            .Take(take)
            .ProjectToType<RoadmapModel>()
            .ToListAsync();
    }

    public Task<List<RoadmapModel>> GetBetterYouCoursesAsync(int skip, int take)
    {
        return _context.Roadmaps
            .Where(r => r.CourseTypes
                .Any(rt => rt.Type.Name == BetterYouTypeName))
            .OrderByDescending(r => r.Created)
            .Skip(skip)
            .Take(take)
            .ProjectToType<RoadmapModel>()
            .ToListAsync();
    }

    public Task<List<RoadmapModel>> GetWelcomeCoursesAsync(int skip, int take)
    {
        return _context.Roadmaps
            .Where(r => r.CourseTypes
                .Any(rt => rt.Type.Name == WelcomeTypeName))
            .OrderByDescending(r => r.Created)
            .Take(10)
            .ProjectToType<RoadmapModel>()
            .ToListAsync();
    }

    public Task<List<CategoryModel>> GetCategoriesAsync()
    {
        return _context.Categories
            .Where(c => !c.ParentRelations.Any())
            .OrderBy(c => c.Position)
            .ProjectToType<CategoryModel>()
            .ToListAsync();
    }
}