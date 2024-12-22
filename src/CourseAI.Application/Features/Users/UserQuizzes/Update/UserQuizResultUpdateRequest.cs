using System.Text.Json.Serialization;
using CourseAI.Application.Core;
using CourseAI.Application.Models.Roadmaps;

namespace CourseAI.Application.Features.Users.UserQuizzes.Update;

public class UserQuizResultUpdateRequest : UserQuizModelBase, IRequestModel
{
    [JsonIgnore]
    public long UserId { get; set; }
}