using Fleet.Application.Core;
using Fleet.Application.Models;
using Fleet.Application.Models.UserRoadmaps;
using Fleet.Domain.Context;
using Fleet.Domain.Entities;
using Mapster;
using OneOf;

namespace Fleet.Application.Features.Users.UserRoadmaps.Add
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
