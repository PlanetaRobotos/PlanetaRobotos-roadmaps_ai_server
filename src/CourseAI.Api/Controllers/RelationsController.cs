using CourseAI.Api.Core;
using CourseAI.Application.Models.Categories;
using CourseAI.Application.Services;
using CourseAI.Domain.Context;
using CourseAI.Domain.Entities.Categories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CourseAI.Api.Controllers;

public class RelationsController : V1Controller
{
    private readonly AppDbContext _context;
    private readonly IPositionService _positionService;

    public RelationsController(AppDbContext context, IPositionService positionService)
    {
        _context = context;
        _positionService = positionService;
    }

    // Category-Roadmap Relations
    [HttpPost("categories/{categoryId}/roadmaps/{roadmapId}")]
    public async Task<IActionResult> AddRoadmapToCategory(
        Guid categoryId,
        Guid roadmapId,
        [FromBody] PositionRequest request)
    {
        var exists = await _context.CategoryCourses
            .AnyAsync(cr => cr.CategoryId == categoryId && cr.RoadmapId == roadmapId);

        if (exists)
            return Conflict("This relation already exists");
    
        var query = _context.CategoryCourses
            .Where(cc => cc.CategoryId == categoryId);

        var categoryRoadmap = new CategoryCourse
        {
            CategoryId = categoryId,
            RoadmapId = roadmapId,
        };

        await using var transaction = await _context.Database.BeginTransactionAsync();
        try 
        {    
            await _positionService.InsertAtPositionAsync(
                query,
                categoryRoadmap, 
                request.Position);

            _context.CategoryCourses.Add(categoryRoadmap);
            await _context.SaveChangesAsync();
        
            await transaction.CommitAsync();
            return Ok();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return BadRequest("Failed to add roadmap to category: " + ex.Message);
        }
    }
    
    [HttpPut("categories/{categoryId}/roadmaps/reorder")]
    public async Task<IActionResult> ReorderCategoryRoadmaps(
        Guid categoryId,
        Dictionary<Guid, int> positions)
    {
        var query = _context.CategoryCourses
            .Where(cc => cc.CategoryId == categoryId);

        await _positionService.ReorderAsync(query, positions);
        return Ok();
    }

    [HttpPost("categories/{categoryId}/roadmaps/normalize")]
    public async Task<IActionResult> NormalizePositions(Guid categoryId)
    {
        var query = _context.CategoryCourses
            .Where(cc => cc.CategoryId == categoryId);

        await _positionService.NormalizePositionsAsync(query);
        return Ok();
    }

    [HttpDelete("categories/{categoryId}/roadmaps/{roadmapId}")]
    public async Task<IActionResult> RemoveRoadmapFromCategory(Guid categoryId, Guid roadmapId)
    {
        var relation = await _context.CategoryCourses
            .FirstOrDefaultAsync(cr =>
                cr.CategoryId == categoryId &&
                cr.RoadmapId == roadmapId);

        if (relation == null)
            return NotFound();

        _context.CategoryCourses.Remove(relation);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // Category Parent-Child Relations
    [HttpPost("categories/{parentId}/subcategories/{childId}")]
    public async Task<IActionResult> AddSubcategory(
        Guid parentId,
        Guid childId,
        [FromBody] AddCategoryRelationRequest request)
    {
        if (parentId == childId)
            return BadRequest("A category cannot be its own subcategory");

        var exists = await _context.CategoryRelations
            .AnyAsync(cr =>
                cr.ParentCategoryId == parentId &&
                cr.ChildCategoryId == childId);

        if (exists)
            return Conflict("This relation already exists");

        // Check for circular reference
        var hasCircularReference = await HasCircularReference(parentId, childId);
        if (hasCircularReference)
            return BadRequest("This would create a circular reference");

        var categoryRelation = new CategoryRelation
        {
            ParentCategoryId = parentId,
            ChildCategoryId = childId,
            // Order = request.Order
        };

        _context.CategoryRelations.Add(categoryRelation);

        try
        {
            await _context.SaveChangesAsync();
            return Ok();
        }
        catch (DbUpdateException)
        {
            return BadRequest("Invalid category IDs");
        }
    }

    [HttpDelete("categories/{parentId}/subcategories/{childId}")]
    public async Task<IActionResult> RemoveSubcategory(Guid parentId, Guid childId)
    {
        var relation = await _context.CategoryRelations
            .FirstOrDefaultAsync(cr =>
                cr.ParentCategoryId == parentId &&
                cr.ChildCategoryId == childId);

        if (relation == null)
            return NotFound();

        _context.CategoryRelations.Remove(relation);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // Roadmap-Type Relations
    [HttpPost("types/{typeId}/roadmaps/{roadmapId}")]
    public async Task<IActionResult> AddTypeToRoadmap(Guid typeId, Guid roadmapId)
    {
        var exists = await _context.CourseTypeRelations
            .AnyAsync(rt => rt.TypeId == typeId && rt.RoadmapId == roadmapId);

        if (exists)
            return Conflict("This relation already exists");

        var typeRelation = new CourseTypeRelation
        {
            TypeId = typeId,
            RoadmapId = roadmapId
        };

        _context.CourseTypeRelations.Add(typeRelation);

        try
        {
            await _context.SaveChangesAsync();
            return Ok();
        }
        catch (DbUpdateException)
        {
            return BadRequest("Invalid type or roadmap ID");
        }
    }

    [HttpDelete("types/{typeId}/roadmaps/{roadmapId}")]
    public async Task<IActionResult> RemoveTypeFromRoadmap(Guid typeId, Guid roadmapId)
    {
        var relation = await _context.CourseTypeRelations
            .FirstOrDefaultAsync(rt =>
                rt.TypeId == typeId &&
                rt.RoadmapId == roadmapId);

        if (relation == null)
            return NotFound();

        _context.CourseTypeRelations.Remove(relation);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // Helper methods
    private async Task<bool> HasCircularReference(Guid parentId, Guid childId)
    {
        var visited = new HashSet<Guid>();
        return await HasCircularReferenceRecursive(childId, parentId, visited);
    }

    private async Task<bool> HasCircularReferenceRecursive(
        Guid currentId,
        Guid targetId,
        HashSet<Guid> visited)
    {
        if (!visited.Add(currentId))
            return false;

        var childCategories = await _context.CategoryRelations
            .Where(cr => cr.ParentCategoryId == currentId)
            .Select(cr => cr.ChildCategoryId)
            .ToListAsync();

        if (childCategories.Contains(targetId))
            return true;

        foreach (var childId in childCategories)
        {
            if (await HasCircularReferenceRecursive(childId, targetId, visited))
                return true;
        }

        return false;
    }
}