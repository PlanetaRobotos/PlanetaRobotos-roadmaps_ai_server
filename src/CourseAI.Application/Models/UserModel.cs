using CourseAI.Domain.Entities.Identity;

namespace CourseAI.Application.Models;

public class UserModel : UserModelBase
{
    public long Id { get; set; }
}

public class UserModelBase
{
    public string FirstName { get; set; }
    public string Email { get; set; }
    public bool EmailConfirmed { get; set; }

    public User ToEntity() =>
        new()
        {
            FirstName = FirstName,
            Email = Email,
            EmailConfirmed = EmailConfirmed
        };
}
