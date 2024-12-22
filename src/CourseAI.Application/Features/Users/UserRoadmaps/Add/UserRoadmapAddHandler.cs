using CourseAI.Application.Core;
using CourseAI.Application.Models;
using CourseAI.Application.Models.UserRoadmaps;
using CourseAI.Domain.Context;
using CourseAI.Domain.Entities;
using Mapster;
using Microsoft.EntityFrameworkCore;
using OneOf;

namespace CourseAI.Application.Features.Users.UserRoadmaps.Add
{
    public class UserRoadmapAddHandler(AppDbContext dbContext)
        : IHandler<UserRoadmapAddRequest, UserRoadmapModel>
    {
        public async ValueTask<OneOf<UserRoadmapModel, Error>> Handle(UserRoadmapAddRequest request, CancellationToken ct)
        {
            var userRoadmap = request.ToEntity();

            var roadmap = await dbContext.Roadmaps
                .Where(e => e.Id == request.RoadmapId)
                .Include(e => e.Modules)
                .ThenInclude(m => m.Lessons)
                .ThenInclude(l => l.Quizzes)
                .FirstOrDefaultAsync(ct);

            var exists = await dbContext.UserRoadmaps
                .AnyAsync(ur => ur.UserId == request.UserId && ur.RoadmapId == request.RoadmapId, ct);

            if (!exists)
            {
                dbContext.UserRoadmaps.Add(new UserRoadmap
                {
                    UserId = request.UserId,
                    RoadmapId = userRoadmap.RoadmapId,
                    Roadmap = roadmap
                });

                await dbContext.SaveChangesAsync(ct);
            }
            else
            {
                return Error.ServerError($"User roadmap with RoadmapID '{request.RoadmapId}' and UsrId '{request.UserId}' already exists.");
            }

            return userRoadmap.Adapt<UserRoadmapModel>();
        }
    }
}
