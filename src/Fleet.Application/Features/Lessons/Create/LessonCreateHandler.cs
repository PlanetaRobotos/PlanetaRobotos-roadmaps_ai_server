using Fleet.Application.Core;
using Fleet.Application.Models;
using Fleet.Application.Models.Roadmaps;
using Fleet.Domain.Context;
using Mapster;
using OneOf;

namespace Fleet.Application.Features.Lessons.Create;

public class LessonCreateHandler(AppDbContext dbContext) : IHandler<LessonCreateRequest, LessonModel>
{
    public async ValueTask<OneOf<LessonModel, Error>> Handle(LessonCreateRequest request, CancellationToken ct)
    {
        var Lesson = request.ToEntity();

        dbContext.Lessons.Add(Lesson);
        await dbContext.SaveChangesAsync(ct);

        return Lesson.Adapt<LessonModel>();
    }
}
