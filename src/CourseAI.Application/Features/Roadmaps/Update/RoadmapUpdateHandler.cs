using CourseAI.Application.Core;
using CourseAI.Application.Models;
using CourseAI.Domain.Context;
using CourseAI.Domain.Entities;
using CourseAI.Domain.Entities.Roadmaps;
using Mediator;
using Microsoft.EntityFrameworkCore;
using OneOf;

namespace CourseAI.Application.Features.Roadmaps.Update;

public class RoadmapUpdateHandler(AppDbContext dbContext) : IHandler<RoadmapUpdateRequest>
{
    public async ValueTask<OneOf<Unit, Error>> Handle(RoadmapUpdateRequest request, CancellationToken ct)
    {
        var roadmap = await dbContext.Roadmaps
            .Include(r => r.Modules)
            .ThenInclude(m => m.Lessons) // Ensure steps are loaded
            .FirstOrDefaultAsync(r => r.Id == request.Id, ct);

        if (roadmap is null)
        {
            return Error.NotFound<Roadmap>();
        }

        // Update the roadmap fields if provided
        if (!string.IsNullOrWhiteSpace(request.Title))
        {
            roadmap.Title = request.Title;
        }

        if (!string.IsNullOrWhiteSpace(request.Topic))
        {
            roadmap.Topic = request.Topic;
        }

        if (request.EstimatedDuration.HasValue)
        {
            roadmap.EstimatedDuration = request.EstimatedDuration.Value;
        }

        // Check if a step update is requested
        if (request is { LessonId: not null, LessonCompleted: not null, })
        {
            // Find the step
            var step = roadmap.Modules
                .SelectMany(m => m.Lessons)
                .FirstOrDefault(s => s.Id == request.LessonId.Value);

            if (step is null)
            {
                return Error.NotFound($"Step with ID {request.LessonId} not found.");
            }

            // Update step completion status
            step.Completed = request.LessonCompleted.Value;
        }

        await dbContext.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
