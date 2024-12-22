using CourseAI.Application.Core;
using CourseAI.Application.Models;
using CourseAI.Domain.Context;
using CourseAI.Domain.Entities;
using CourseAI.Domain.Entities.Identity;
using Mediator;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OneOf;

namespace CourseAI.Application.Features.Users.UserRoadmaps.Delete;

public class UserRoadmapDeleteHandler(AppDbContext dbContext) : IHandler<UserRoadmapDeleteRequest>
{
    public async ValueTask<OneOf<Unit, Error>> Handle(UserRoadmapDeleteRequest request, CancellationToken ct)
    {
        var userRoadmap = await dbContext.UserRoadmaps
            .FirstOrDefaultAsync(ur => ur.UserId == request.UserId && ur.RoadmapId == request.RoadmapId, ct);

        if (userRoadmap is null)
        {
            return Error.NotFound<UserRoadmap>();
        }
        
        dbContext.UserRoadmaps.Remove(userRoadmap);
        await dbContext.SaveChangesAsync(ct);
        
        return Unit.Value;
    }
}
