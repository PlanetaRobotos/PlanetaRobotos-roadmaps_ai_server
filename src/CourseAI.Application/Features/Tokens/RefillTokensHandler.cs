using CourseAI.Application.Core;
using CourseAI.Application.Models;
using CourseAI.Application.Services;
using CourseAI.Core.Enums;
using CourseAI.Core.Security;
using CourseAI.Domain.Context;
using CourseAI.Domain.Entities.Identity;
using CourseAI.Domain.Entities.Transactions;
using Mediator;
using Microsoft.AspNetCore.Identity;
using OneOf;

namespace CourseAI.Application.Features.Tokens;

public class RefillTokensHandler(UserManager<User> userManager, IUserService userService, AppDbContext dbContext)
    : IHandler<RefillTokensRequest>
{
    public async ValueTask<OneOf<Unit, Error>> Handle(RefillTokensRequest request, CancellationToken ct)
    {
        var userResult = await userService.GetUser();
        var user = userResult.Match(
            user => user,
            error => throw new Exception(error.Message)
        );
        
        var roles = await userManager.GetRolesAsync(user);

        if (roles.Contains(Roles.Standard) || roles.Contains(Roles.Enterprise))
            return Error.ServerError("You need to have a plan to refill tokens.");
        
        user.Tokens += request.Amount;
        
        var result = await userManager.UpdateAsync(user);
        if (!result.Succeeded)
            return Error.ServerError("Failed to update user tokens.");

        var tokenTransaction = new TokenTransaction
        {
            UserId = request.UserId,
            Amount = request.Amount,
            TransactionType = TransactionType.Refill,
        };
        
        await dbContext.TokenTransactions.AddAsync(tokenTransaction, ct);
        await dbContext.SaveChangesAsync(ct);
        
        return Unit.Value;
    }
}