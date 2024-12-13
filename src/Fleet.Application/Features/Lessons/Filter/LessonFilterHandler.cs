using Fleet.Application.Core;
using Fleet.Application.Models;
using Fleet.Application.Models.Roadmaps;
using Fleet.Application.Models.Shared;
using Fleet.Domain.Context;
using Mapster;
using Microsoft.EntityFrameworkCore;
using OneOf;

namespace Fleet.Application.Features.Lessons.Filter;

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
