using CourseAI.Domain.Entities.Identity;

namespace CourseAI.Application.Models;

public class UserModel : UserModelBase
{
    public long Id { get; set; }
}

public class UserModelBase
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Bio { get; set; }
    public bool EmailConfirmed { get; set; }

    public User ToEntity() =>
        new()
        {
            Name = Name,
            Email = Email,
            EmailConfirmed = EmailConfirmed,
            Bio = Bio
        };
}
