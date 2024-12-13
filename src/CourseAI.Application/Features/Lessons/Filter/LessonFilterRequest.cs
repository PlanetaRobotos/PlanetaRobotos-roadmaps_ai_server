using CourseAI.Application.Core;
using CourseAI.Application.Models.Roadmaps;
using CourseAI.Application.Models.Shared;

namespace CourseAI.Application.Features.Lessons.Filter;

public class LessonFilterRequest : FilterRequestBase<LessonFilterRequest>, IRequestModel<Filtered<LessonModel>>;
