using CourseAI.Application.Core;
using CourseAI.Application.Models;
using CourseAI.Application.Models.Roadmaps;
using CourseAI.Application.Models.Shared;
using CourseAI.Domain.Context;
using Mapster;
using Microsoft.EntityFrameworkCore;
using OneOf;

namespace CourseAI.Application.Features.Lessons.Filter;

public class LessonFilterHandler(AppDbContext dbContext) : IHandler<LessonFilterRequest, Filtered<LessonModel>>
{
    public async ValueTask<OneOf<Filtered<LessonModel>, Error>> Handle(LessonFilterRequest request, CancellationToken ct)
    {
        var Lessons = await dbContext.Lessons.ToArrayAsync(ct);

        return new Filtered<LessonModel>
        {
            Data = Lessons.Select(c => c.Adapt<LessonModel>()).ToArray(),
            Total = Lessons.Length,
            Columns = null,
        };
    }
}
