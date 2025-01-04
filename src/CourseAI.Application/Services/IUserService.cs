using CourseAI.Application.Models;
using CourseAI.Domain.Entities.Identity;
using OneOf;

namespace CourseAI.Application.Services;

public interface IUserService
{
    Task<bool> CreateUser(string email, bool emailConfirmed, string? role);
    ValueTask<OneOf<User, Error>> GetUser();
}