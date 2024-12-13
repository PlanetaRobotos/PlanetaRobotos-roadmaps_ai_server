using CourseAI.Application.Core;
using CourseAI.Application.Models;
using CourseAI.Domain.Context;
using CourseAI.Domain.Entities.Roadmaps;
using Mediator;
using Microsoft.EntityFrameworkCore;
using OneOf;

namespace CourseAI.Application.Features.Lessons.Update;

public class LessonUpdateHandler(AppDbContext dbContext) : IHandler<LessonUpdateRequest>
{
    public async ValueTask<OneOf<Unit, Error>> Handle(LessonUpdateRequest request, CancellationToken ct)
    {
        var Lesson = await dbContext.Lessons
            .FirstOrDefaultAsync(r => r.Id == request.Id, ct);

        if (Lesson is null)
        {
            return Error.NotFound<Lesson>();
        }

        if (request is { LessonId: not null, LessonCompleted: not null, })
        {
            Lesson.Completed = request.LessonCompleted.Value;
        }

        await dbContext.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
