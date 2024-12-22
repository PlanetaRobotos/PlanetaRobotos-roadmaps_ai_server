using CourseAI.Application.Core;
using CourseAI.Application.Models;
using CourseAI.Domain.Context;
using CourseAI.Domain.Entities;
using Mediator;
using Microsoft.EntityFrameworkCore;
using OneOf;

namespace CourseAI.Application.Features.Users.UserQuizzes.Update;

public class UserQuizResultUpdateHandler(AppDbContext dbContext) : IHandler<UserQuizResultUpdateRequest>
{
    public async ValueTask<OneOf<Unit, Error>> Handle(UserQuizResultUpdateRequest request, CancellationToken ct)
    {
        var userQuiz = await dbContext.UserQuizzes
            .FirstOrDefaultAsync(ur => ur.UserId == request.UserId && ur.QuizId == request.QuizId, ct);

        if (userQuiz is null)
        {
            dbContext.UserQuizzes.Add(new UserQuiz
            {
                UserId = request.UserId,
                QuizId = request.QuizId,
                AnswerIndex = request.AnswerIndex,
            });
        }

        await dbContext.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
