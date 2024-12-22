using CourseAI.Application.Core;
using CourseAI.Application.Models.Shared;
using CourseAI.Application.Models.UserRoadmaps;
using System.Text.Json.Serialization;

namespace CourseAI.Application.Features.Users.UserRoadmaps.Filter;

public class UserRoadmapFilterRequest : FilterRequestBase<UserRoadmapFilterRequest>, IRequestModel<Filtered<UserRoadmapModel>>
{
    [JsonIgnore]
    public long UserId { get; set; }
}