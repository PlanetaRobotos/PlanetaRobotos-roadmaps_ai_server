using CourseAI.Application.Core;
using CourseAI.Application.Models;
using CourseAI.Domain.Context;
using CourseAI.Domain.Entities.Roadmaps;
using Mediator;
using Microsoft.EntityFrameworkCore;
using OneOf;

namespace CourseAI.Application.Features.Roadmaps.UserLikes
{
    public class UserRoadmapDeleteHandler(AppDbContext dbContext) : IHandler<UserLikeDeleteRequest>
    {
        public async ValueTask<OneOf<Unit, Error>> Handle(UserLikeDeleteRequest request, CancellationToken ct)
        {
            var userLike = await dbContext.UserLikes
                .FirstOrDefaultAsync(ur => ur.UserId == request.UserId && ur.RoadmapId == request.RoadmapId, ct);

            if (userLike is null)
            {
                return Error.NotFound<UserLike>();
            }

            dbContext.UserLikes.Remove(userLike);
            await dbContext.SaveChangesAsync(ct);

            return Unit.Value;
        }
    }
}