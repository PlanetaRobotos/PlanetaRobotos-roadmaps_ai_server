using CourseAI.Application.Core;
using CourseAI.Application.Models;
using CourseAI.Application.Models.Shared;
using CourseAI.Application.Models.UserRoadmaps;
using CourseAI.Domain.Context;
using Mapster;
using Microsoft.EntityFrameworkCore;
using OneOf;

namespace CourseAI.Application.Features.Users.UserRoadmaps.Filter;

public class UserRoadmapFilterHandler(AppDbContext dbContext) : IHandler<UserRoadmapFilterRequest, Filtered<UserRoadmapModel>>
{
    public async ValueTask<OneOf<Filtered<UserRoadmapModel>, Error>> Handle(UserRoadmapFilterRequest request, CancellationToken ct)
    {
        var UserRoadmaps = await dbContext.UserRoadmaps
            .Where(ur => ur.UserId == request.UserId)
            .Include(ur => ur.Roadmap)
            .ToArrayAsync(ct);
        
        return new Filtered<UserRoadmapModel>
        {
            Data = UserRoadmaps.Select(c => c.Adapt<UserRoadmapModel>()).ToArray(),
            Total = UserRoadmaps.Length,
            Columns = null,
        };
    }
}