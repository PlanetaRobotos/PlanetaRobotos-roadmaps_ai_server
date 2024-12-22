using CourseAI.Application.Core;
using CourseAI.Application.Models;
using CourseAI.Application.Models.Roadmaps;
using CourseAI.Application.Models.Shared;
using CourseAI.Domain.Context;
using Mapster;
using Microsoft.EntityFrameworkCore;
using OneOf;

namespace CourseAI.Application.Features.Users.UserQuizzes.Filter;

public class UserQuizFilterHandler(AppDbContext dbContext) : IHandler<UserQuizFilterRequest, Filtered<UserQuizModel>>
{
    public async ValueTask<OneOf<Filtered<UserQuizModel>, Error>> Handle(UserQuizFilterRequest request, CancellationToken ct)
    {
        var UserQuizzes = await dbContext.UserQuizzes
            .Where(uq => uq.UserId == request.UserId)
            .Include(uq => uq.Quiz)
            .ToArrayAsync(ct);
        
        return new Filtered<UserQuizModel>
        {
            Data = UserQuizzes.Select(c => c.Adapt<UserQuizModel>()).ToArray(),
            Total = UserQuizzes.Length,
            Columns = null,
        };
    }
}
