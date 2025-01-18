using CourseAI.Application.Core;
using CourseAI.Application.Models.Roadmaps;
using CourseAI.Application.Models.Shared;

namespace CourseAI.Application.Models.Rooms;

public class RoadmapRoomRequest : FilterRequestBase<RoadmapRoomRequest>, IRequestModel<Filtered<RoadmapModel>>
{
    public string RoomType { get; init; }
}