using Fleet.Application.Core;
using Fleet.Application.Models.Roadmaps;
using Fleet.Application.Models.Shared;

namespace Fleet.Application.Features.Lessons.Filter;

public class LessonFilterRequest : FilterRequestBase<LessonFilterRequest>, IRequestModel<Filtered<LessonModel>>;
