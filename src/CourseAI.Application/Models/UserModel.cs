using CourseAI.Domain.Entities.Identity;

namespace CourseAI.Application.Models;

public class UserModel : UserModelBase
{
    public long Id { get; set; }
}

public class UserModelBase
{
    public string UserName { get; set; }
    public string Email { get; set; }

    public User ToEntity() =>
        new()
        {
            UserName = UserName,
            Email = Email
        };
}
