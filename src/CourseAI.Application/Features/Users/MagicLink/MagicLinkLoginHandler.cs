using CourseAI.Application.Core;
using CourseAI.Application.Models;
using CourseAI.Application.Services;
using CourseAI.Domain.Context;
using CourseAI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using OneOf;

namespace CourseAI.Application.Features.Users.MagicLink
{
    public class MagicLinkLoginHandler(AppDbContext dbContext, IJwtProvider jwtProvider) : IHandler<MagicLinkLoginRequest, string>
    {
        public async ValueTask<OneOf<string, Error>> Handle(MagicLinkLoginRequest request, CancellationToken ct)
        {
            EmailVerificationToken? verificationToken = await dbContext.EmailVerificationTokens
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.Id == request.TokenId, ct);

            if (verificationToken == null || verificationToken.ExpiresOnUtc < DateTime.UtcNow)
            {
                return Error.NotFound("Token not found");
            }
            
            verificationToken.User.EmailConfirmed = true;

            var jwtToken = jwtProvider.Create(verificationToken.User);
            
            dbContext.EmailVerificationTokens.Remove(verificationToken);
            await dbContext.SaveChangesAsync(ct);
            
            return jwtToken;
        }
    }
}