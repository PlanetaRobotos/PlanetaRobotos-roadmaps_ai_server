using CourseAI.Application.Core;
using CourseAI.Application.Models;
using CourseAI.Application.Models.Roadmaps;
using CourseAI.Domain.Context;
using Mapster;
using OneOf;

namespace CourseAI.Application.Features.Lessons.Create;

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
