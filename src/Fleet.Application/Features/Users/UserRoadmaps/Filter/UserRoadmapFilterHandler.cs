using Fleet.Application.Core;
using Fleet.Application.Models;
using Fleet.Application.Models.Shared;
using Fleet.Application.Models.UserRoadmaps;
using Fleet.Domain.Context;
using Mapster;
using Microsoft.EntityFrameworkCore;
using OneOf;

namespace Fleet.Application.Features.Users.UserRoadmaps.Filter;

public class UserRoadmapFilterHandler(AppDbContext dbContext) : IHandler<UserRoadmapFilterRequest, Filtered<UserRoadmapModel>>
{
    public async ValueTask<OneOf<Filtered<UserRoadmapModel>, Error>> Handle(UserRoadmapFilterRequest request, CancellationToken ct)
    {
        var UserRoadmaps = await dbContext.UserRoadmaps
            // .Include(ur => ur.Roadmap)
            .Where(ur => ur.UserId == request.UserId)
            .ToArrayAsync(ct);
        
        return new Filtered<UserRoadmapModel>
        {
            Data = UserRoadmaps.Select(c => c.Adapt<UserRoadmapModel>()).ToArray(),
            Total = UserRoadmaps.Length,
            Columns = null,
        };
    }
}
