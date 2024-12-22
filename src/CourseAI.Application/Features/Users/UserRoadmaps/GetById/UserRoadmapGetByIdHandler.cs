using CourseAI.Application.Core;
using CourseAI.Application.Models;
using CourseAI.Application.Models.UserRoadmaps;
using CourseAI.Domain.Context;
using CourseAI.Domain.Entities;
using Mapster;
using Microsoft.EntityFrameworkCore;
using OneOf;

namespace CourseAI.Application.Features.Users.UserRoadmaps.GetById;

public class UserRoadmapGetByIdHandler(AppDbContext dbContext) : IHandler<UserRoadmapGetByIdRequest, UserRoadmapModel?>
{
    public async ValueTask<OneOf<UserRoadmapModel?, Error>> Handle(UserRoadmapGetByIdRequest request, CancellationToken ct)
    {
        var UserRoadmap = await dbContext.UserRoadmaps
            .Where(e => e.UserId == request.UserId && e.RoadmapId == request.RoadmapId)
            .Include(e => e.Roadmap)
            .FirstOrDefaultAsync(ct);

        if (UserRoadmap is null)
        {
            return new OneOf<UserRoadmapModel?, Error>();
        }

        var model = UserRoadmap.Adapt<UserRoadmapModel>();
        return model;
    }
}
