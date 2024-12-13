using Fleet.Application.Core;
using Fleet.Application.Models;
using Fleet.Domain.Context;
using Fleet.Domain.Entities.Roadmaps;
using Mediator;
using OneOf;

namespace Fleet.Application.Features.Lessons.Delete;

public class ClientDeleteHandler(AppDbContext dbContext) : IHandler<LessonDeleteRequest>
{
    public async ValueTask<OneOf<Unit, Error>> Handle(LessonDeleteRequest request, CancellationToken ct)
    {
        var Lesson = await dbContext.Lessons.FindAsync([request.Id,], ct);

        if (Lesson is null)
        {
            return Error.NotFound<Lesson>();
        }

        dbContext.Lessons.Remove(Lesson);
        await dbContext.SaveChangesAsync(ct);

        return Unit.Value;
    }
}
