// using System.Linq.Dynamic.Core;
// using CourseAI.Application.Core;
// using CourseAI.Application.Models.Roadmaps;
// using CourseAI.Application.Models.Shared;
// using CourseAI.Domain.Context;
// using Microsoft.EntityFrameworkCore;
// using OneOf;
//
// namespace CourseAI.Application.Models.Rooms;
//
// public class RoadmapRoomHandler(AppDbContext dbContext) : IHandler<RoadmapRoomRequest, Filtered<RoadmapModel>>
// {
//     public async ValueTask<OneOf<Filtered<RoadmapModel>, Error>> Handle(RoadmapRoomRequest request,
//         CancellationToken ct)
//     {
//         var query = dbContext.Roadmaps
//             .Include(x => x.Modules)
//             .ThenInclude(x => x.Lessons)
//             .ThenInclude(x => x.Quizzes)
//             .AsQueryable();
//
//         // Apply category filtering
//         query = request.RoomType switch
//         {
//             "new-noteworthy" => query.OrderByDescending(x => x.CreatedAt).Take(50),
//             "top-charts" => query.OrderByDescending(x => x.Rating).Take(50),
//             "my-progress" => query.Where(x => x.HasUserProgress),
//             _ => query.Where(x => x.CategorySlug == request.RoomType)
//         };
//
//         // Apply pagination
//         var skip = (request.Page - 1) * request.PageSize;
//         var roadmaps = await query
//             .Skip(skip)
//             .Take(request.PageSize)
//             .ToArrayAsync(ct);
//
//         var total = await query.CountAsync(ct);
//
//         var roadmapModels = roadmaps.Select(r => r.Adapt<RoadmapModel>()).ToArray();
//
//         return new Filtered<RoadmapModel>
//         {
//             Data = roadmapModels,
//             Total = total,
//             Columns = null
//         };
//     }
// }