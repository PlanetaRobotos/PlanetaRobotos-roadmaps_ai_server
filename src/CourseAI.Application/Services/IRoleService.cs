namespace CourseAI.Application.Services;

public interface IRoleService
{
    Task<bool> AssignRoleAsync(long userId, string roleName);
    Task<bool> RemoveRoleAsync(long userId, string roleName);
}