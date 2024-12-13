using Fleet.Application.Core;
using Fleet.Application.Models;
using Fleet.Domain.Context;
using Fleet.Domain.Entities.Roadmaps;
using Mediator;
using Microsoft.EntityFrameworkCore;
using OneOf;

namespace Fleet.Application.Features.Lessons.Update;

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
