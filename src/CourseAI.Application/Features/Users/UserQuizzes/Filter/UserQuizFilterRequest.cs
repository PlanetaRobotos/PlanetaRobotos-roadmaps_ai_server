using System.Text.Json.Serialization;
using CourseAI.Application.Core;
using CourseAI.Application.Models.Roadmaps;
using CourseAI.Application.Models.Shared;

namespace CourseAI.Application.Features.Users.UserQuizzes.Filter;

public class UserQuizFilterRequest : FilterRequestBase<UserQuizFilterRequest>, IRequestModel<Filtered<UserQuizModel>>
{
    [JsonIgnore]
    public long UserId { get; set; }
}