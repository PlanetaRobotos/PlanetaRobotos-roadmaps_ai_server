using CourseAI.Application.Core;
using CourseAI.Application.Models;
using CourseAI.Application.Models.Roadmaps;
using CourseAI.Domain.Context;
using CourseAI.Domain.Entities.Roadmaps;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OneOf;

namespace CourseAI.Application.Features.Lessons.GetById;

public class LessonGetByIdHandler(AppDbContext dbContext) : IHandler<LessonGetByIdRequest, LessonModel>
{
    public async ValueTask<OneOf<LessonModel, Error>> Handle(LessonGetByIdRequest request, CancellationToken ct)
    {
        var lesson = await dbContext.Lessons
            .Where(e => e.Id == request.Id)
            .FirstOrDefaultAsync(ct);

        if (lesson is null)
        {
            return Error.NotFound<Lesson>();
        }

        await dbContext.SaveChangesAsync(ct);

        return lesson.Adapt<LessonModel>();
    }
}
