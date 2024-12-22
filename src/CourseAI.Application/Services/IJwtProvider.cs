using CourseAI.Domain.Entities.Identity;

namespace CourseAI.Application.Services;

public interface IJwtProvider
{
    string Create(User user);
}