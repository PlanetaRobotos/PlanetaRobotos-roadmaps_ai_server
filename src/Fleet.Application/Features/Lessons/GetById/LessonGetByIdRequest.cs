using Fleet.Application.Core;
using Fleet.Application.Models.Roadmaps;

namespace Fleet.Application.Features.Lessons.GetById;

public class LessonGetByIdRequest : IRequestModel<LessonModel>
{
    public Guid Id { get; init; }
}
