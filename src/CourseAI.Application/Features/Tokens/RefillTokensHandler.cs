using System.Security.Claims;
using CourseAI.Application.Core;
using CourseAI.Application.Models;
using CourseAI.Core.Enums;
using CourseAI.Domain.Context;
using CourseAI.Domain.Entities.Identity;
using CourseAI.Domain.Entities.Transactions;
using Mediator;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using OneOf;

namespace CourseAI.Application.Features.Tokens;

public class RefillTokensHandler(IHttpContextAccessor httpContextAccessor, UserManager<User> userManager, AppDbContext dbContext)
    : IHandler<RefillTokensRequest>
{
    public async ValueTask<OneOf<Unit, Error>> Handle(RefillTokensRequest request, CancellationToken ct)
    {
        var httpUser = httpContextAccessor.HttpContext?.User;
        if (httpUser == null)
            return Error.Unauthorized("User not found from httpContextAccessor");

        var userId = httpUser.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
            return Error.Unauthorized("User not found by ClaimTypes.NameIdentifier");

        var user = await userManager.FindByIdAsync(userId);
        if (user == null)
            return Error.Unauthorized("User not found from userManager");
        
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