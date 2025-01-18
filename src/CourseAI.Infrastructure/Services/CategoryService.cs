using CourseAI.Application.Models.Categories;
using CourseAI.Application.Models.Roadmaps;
using CourseAI.Application.Services;
using CourseAI.Domain.Context;
using Mapster;
using Microsoft.EntityFrameworkCore;
using NeerCore.DependencyInjection;
using NeerCore.Exceptions;

namespace CourseAI.Infrastructure.Services;

[Service]
public sealed class CategoryService : ICategoryService
{
    private readonly AppDbContext _context;
    private readonly IDbContextFactory<AppDbContext> _contextFactory;

    public CategoryService(AppDbContext context, IDbContextFactory<AppDbContext> contextFactory)
    {
        _context = context;
        _contextFactory = contextFactory;
    }

    public Task<List<CategoryModel>> GetCategoriesAsync(Guid? parentId)
    {
        var query = parentId.HasValue
            ? _context.Categories
                .Where(c => c.ParentRelations
                    .Any(pr => pr.ParentCategoryId == parentId))
            : _context.Categories
                .Where(c => !c.ParentRelations.Any());

        return query
            .OrderBy(c => c.Position)
            .ProjectToType<CategoryModel>()
            .ToListAsync();
    }

public async Task<CategoryPageModel> GetCategoryPageAsync(Guid id)
{
    await using var context1 = await _contextFactory.CreateDbContextAsync();
    await using var context2 = await _contextFactory.CreateDbContextAsync();
    await using var context3 = await _contextFactory.CreateDbContextAsync();
    await using var context4 = await _contextFactory.CreateDbContextAsync();
    await using var context5 = await _contextFactory.CreateDbContextAsync();

    // Get category details
    var category = await context1.Categories
        .Where(c => c.Id == id)
        .ProjectToType<CategoryModel>()
        .FirstOrDefaultAsync()
        ?? throw new NotFoundException($"Category {id} not found");

    // Get top courses
    var topCoursesTask = context2.Roadmaps
        .Where(r => r.CategoryCourses
            .Any(cr => cr.CategoryId == id))
        .OrderByDescending(r => r.Likes)
        .Take(10)
        .ProjectToType<RoadmapModel>()
        .ToListAsync();

    // Get new courses
    var newCoursesTask = context3.Roadmaps
        .Where(r => r.CategoryCourses
            .Any(cr => cr.CategoryId == id))
        .OrderByDescending(r => r.Created)
        .Take(10)
        .ProjectToType<RoadmapModel>()
        .ToListAsync();

    // Get parent category ID and related categories (siblings)
    var parentCategoryId = await context4.CategoryRelations
        .Where(cr => cr.ChildCategoryId == id)
        .Select(cr => cr.ParentCategoryId)
        .FirstOrDefaultAsync();

    var relatedCategoriesTask = context4.CategoryRelations
        .Where(cr => cr.ParentCategoryId == parentCategoryId
                     && cr.ChildCategoryId != id)
        .Select(cr => cr.ChildCategory)
        .OrderBy(c => c.Position)
        .ProjectToType<CategorySliderModel>()
        .ToListAsync();

    // Get child categories
    var childCategoriesTask = context5.CategoryRelations
        .Where(cr => cr.ParentCategoryId == id)  // Where current category is the parent
        .Select(cr => cr.ChildCategory)
        .OrderBy(c => c.Position)
        .ProjectToType<CategorySliderModel>()
        .ToListAsync();

    // Wait for all parallel tasks to complete
    await Task.WhenAll(
        topCoursesTask,
        newCoursesTask,
        relatedCategoriesTask,
        childCategoriesTask
    );

    return new CategoryPageModel
    {
        Category = category,
        TopCourses = await topCoursesTask,
        NewCourses = await newCoursesTask,
        RelatedCategories = await relatedCategoriesTask,
        ChildCategories = await childCategoriesTask
    };
}
    public Task<List<RoadmapModel>> GetCategoryCoursesAsync(
        Guid id,
        int skip,
        int take,
        string sortBy)
    {
        var query = _context.Roadmaps
            .Where(r => r.CategoryCourses
                .Any(cr => cr.CategoryId == id));

        query = sortBy switch
        {
            "top" => query.OrderByDescending(r => r.Likes),
            "new" => query.OrderByDescending(r => r.Created),
            _ => query.OrderByDescending(r => r.Created) // default to new
        };

        return query
            .Skip(skip)
            .Take(take)
            .ProjectToType<RoadmapModel>()
            .ToListAsync();
    }
}