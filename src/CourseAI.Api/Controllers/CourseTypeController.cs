using CourseAI.Api.Core;
using CourseAI.Application.Models.Categories;
using CourseAI.Application.Models.Roadmaps;
using CourseAI.Domain.Context;
using CourseAI.Domain.Entities.Categories;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CourseAI.Api.Controllers;

public class CourseTypeController: V1Controller
{
    private readonly AppDbContext _context;

    public CourseTypeController(AppDbContext context) => 
        _context = context;

    [HttpPost]
    public async Task<ActionResult<CourseTypeModel>> Create(CourseTypeCreateModel model)
    {
        var courseType = new CourseType
        {
            Name = model.Name,
            Description = model.Description,
            Order = model.Order
        };

        _context.CourseTypes.Add(courseType);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetById), 
            new { id = courseType.Id }, 
            courseType.Adapt<CourseTypeModel>());
    }

    [HttpGet]
    public async Task<ActionResult<List<CourseTypeModel>>> GetAll()
    {
        var types = await _context.CourseTypes
            .OrderBy(t => t.Order)
            .ProjectToType<CourseTypeModel>()
            .ToListAsync();

        return Ok(types);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CourseTypeModel>> GetById(Guid id)
    {
        var courseType = await _context.CourseTypes
            .ProjectToType<CourseTypeModel>()
            .FirstOrDefaultAsync(t => t.Id == id);

        if (courseType == null)
            return NotFound();

        return Ok(courseType);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, CourseTypeUpdateModel model)
    {
        var courseType = await _context.CourseTypes.FindAsync(id);
        if (courseType == null)
            return NotFound();

        courseType.Name = model.Name;
        courseType.Description = model.Description;
        courseType.Order = model.Order;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await CourseTypeExists(id))
                return NotFound();
            throw;
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var courseType = await _context.CourseTypes.FindAsync(id);
        if (courseType == null)
            return NotFound();

        _context.CourseTypes.Remove(courseType);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // Extra endpoints for managing roadmaps in types
    [HttpGet("{id}/roadmaps")]
    public async Task<ActionResult<List<RoadmapModel>>> GetRoadmaps(Guid id)
    {
        var exists = await CourseTypeExists(id);
        if (!exists)
            return NotFound();

        var roadmaps = await _context.CourseTypeRelations
            .Where(rt => rt.TypeId == id)
            .OrderBy(rt => rt.Created)
            .Select(rt => rt.Roadmap)
            .ProjectToType<RoadmapModel>()
            .ToListAsync();

        return Ok(roadmaps);
    }

    private Task<bool> CourseTypeExists(Guid id)
    {
        return _context.CourseTypes.AnyAsync(t => t.Id == id);
    }
}