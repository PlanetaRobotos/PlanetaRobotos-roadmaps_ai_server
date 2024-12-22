using CourseAI.Application.Core;
using CourseAI.Application.Models;
using CourseAI.Application.Models.UserLikes;
using CourseAI.Domain.Context;
using CourseAI.Domain.Entities.Roadmaps;
using Mapster;
using Microsoft.EntityFrameworkCore;
using OneOf;

namespace CourseAI.Application.Features.Roadmaps.UserLikes;

public class UserLikeGetByIdHandler(AppDbContext dbContext) : IHandler<UserLikeGetByIdRequest, UserLikeModel?>
{
    public async ValueTask<OneOf<UserLikeModel?, Error>> Handle(UserLikeGetByIdRequest request, CancellationToken ct)
    {
        var UserLike = await dbContext.UserLikes
            .Where(e => e.UserId == request.UserId && e.RoadmapId == request.RoadmapId)
            .FirstOrDefaultAsync(ct);

        if (UserLike is null)
        {
            return new OneOf<UserLikeModel?, Error>();
        }

        var model = UserLike.Adapt<UserLikeModel>();
        return model;
    }
}
