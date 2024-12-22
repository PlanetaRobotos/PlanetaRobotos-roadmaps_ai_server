using CourseAI.Application.Core;
using CourseAI.Application.Models;
using CourseAI.Application.Models.UserLikes;
using CourseAI.Domain.Context;
using Mapster;
using Microsoft.EntityFrameworkCore;
using OneOf;

namespace CourseAI.Application.Features.Roadmaps.UserLikes
{
    public class UserLikeAddHandler(AppDbContext dbContext)
        : IHandler<UserLikeAddRequest, UserLikeModel>
    {
        public async ValueTask<OneOf<UserLikeModel, Error>> Handle(UserLikeAddRequest request, CancellationToken ct)
        {
            var UserLike = request.ToEntity();
            
            var exists = await dbContext.UserLikes
                .AnyAsync(ur => ur.UserId == request.UserId && ur.RoadmapId == request.RoadmapId, ct);

            if (!exists)
            {
                dbContext.UserLikes.Add(new Domain.Entities.Roadmaps.UserLike
                {
                    UserId = UserLike.UserId,
                    RoadmapId = UserLike.RoadmapId,
                });

                await dbContext.SaveChangesAsync(ct);
            }
            else
            {
                return Error.ServerError($"User roadmap with RoadmapID '{request.RoadmapId}' and UsrId '{request.UserId}' already exists.");
            }

            return UserLike.Adapt<UserLikeModel>();
        }
    }
}
