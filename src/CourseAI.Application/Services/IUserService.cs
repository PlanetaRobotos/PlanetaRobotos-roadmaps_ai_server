using CourseAI.Application.Models;
using CourseAI.Core.Constants;
using CourseAI.Domain.Entities.Identity;
using OneOf;

namespace CourseAI.Application.Services;

public interface IUserService
{
    Task<User?> CreateUser(string email, bool emailConfirmed, string[]? roles,
        int tokensAmount = SharedConstants.DefaultTokensAmount);
    ValueTask<OneOf<User, Error>> GetUser();
}