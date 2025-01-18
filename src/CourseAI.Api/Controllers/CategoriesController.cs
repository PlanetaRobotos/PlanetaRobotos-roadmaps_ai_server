using CourseAI.Api.Core;
using CourseAI.Application.Models.Categories;
using CourseAI.Application.Models.Roadmaps;
using CourseAI.Application.Services;
using CourseAI.Domain.Context;
using CourseAI.Domain.Entities.Categories;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CourseAI.Api.Controllers;

public class CategoriesController(ICategoryService categoryService, AppDbContext context) : V1Controller
{
    [HttpGet]
    public Task<List<CategoryModel>> GetCategories(
        [FromQuery] Guid? parentId)
    {
        return categoryService.GetCategoriesAsync(parentId);
    }

    [HttpGet("page/{id}")]
    [ProducesResponseType<CategoryPageModel>(StatusCodes.Status200OK)]
    public Task<CategoryPageModel> GetCategory(Guid id)
    {
        return categoryService.GetCategoryPageAsync(id);
    }

    [HttpGet("{id}/courses")]
    public Task<List<RoadmapModel>> GetCategoryCourses(
        Guid id,
        [FromQuery] int skip,
        [FromQuery] int take,
        [FromQuery] string sortBy = "new")
    {
        return categoryService.GetCategoryCoursesAsync(id, skip, take, sortBy);
    }
    
    //CRUD operations
    [HttpPost]
    public async Task<ActionResult<CategoryModel>> Create(CategoryCreateModel model)
    {
        var category = new Category
        {
            Title = model.Title,
            Description = model.Description,
            ColorHex = model.ColorHex,
            Order = model.Order
        };

        context.Categories.Add(category);
        await context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetById), 
            new { id = category.Id }, 
            category.Adapt<CategoryModel>());
    }

    [HttpGet("all")]
    public async Task<ActionResult<List<CategoryModel>>> GetAll([FromQuery] Guid? parentId = null)
    {
        var query = context.Categories.AsQueryable();

        if (parentId.HasValue)
        {
            query = query.Where(c => c.ParentRelations
                .Any(pr => pr.ParentCategoryId == parentId));
        }
        else
        {
            query = query.Where(c => !c.ParentRelations.Any());
        }

        var categories = await query
            .OrderBy(c => c.Position)
            .ProjectToType<CategoryModel>()
            .ToListAsync();

        return Ok(categories);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CategoryModel>> GetById(Guid id)
    {
        var category = await context.Categories
            .ProjectToType<CategoryModel>()
            .FirstOrDefaultAsync(c => c.Id == id);

        if (category == null)
            return NotFound();

        return Ok(category);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, CategoryUpdateModel model)
    {
        var category = await context.Categories.FindAsync(id);
        if (category == null)
            return NotFound();

        category.Title = model.Title;
        category.Description = model.Description;
        category.ColorHex = model.ColorHex;
        category.Order = model.Order;

        try
        {
            await context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await CategoryExists(id))
                return NotFound();
            throw;
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var category = await context.Categories.FindAsync(id);
        if (category == null)
            return NotFound();

        context.Categories.Remove(category);
        await context.SaveChangesAsync();

        return NoContent();
    }

    private Task<bool> CategoryExists(Guid id)
    {
        return context.Categories.AnyAsync(c => c.Id == id);
    }
}