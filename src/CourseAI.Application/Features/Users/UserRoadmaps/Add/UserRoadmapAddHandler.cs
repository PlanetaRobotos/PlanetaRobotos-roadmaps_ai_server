using CourseAI.Application.Core;
using CourseAI.Application.Models;
using CourseAI.Application.Models.UserRoadmaps;
using CourseAI.Domain.Context;
using CourseAI.Domain.Entities;
using Mapster;
using OneOf;

namespace CourseAI.Application.Features.Users.UserRoadmaps.Add
{
    public class UserRoadmapAddHandler(AppDbContext dbContext)
        : IHandler<UserRoadmapAddRequest, UserRoadmapModel>
    {
        public async ValueTask<OneOf<UserRoadmapModel, Error>> Handle(UserRoadmapAddRequest request, CancellationToken ct)
        {
            var userRoadmap = request.ToEntity();

            dbContext.UserRoadmaps.Add(new UserRoadmap
            {
                UserId = request.UserId,
                RoadmapId = userRoadmap.RoadmapId,
            });

            await dbContext.SaveChangesAsync(ct);

            return userRoadmap.Adapt<UserRoadmapModel>();
        }
    }
}
