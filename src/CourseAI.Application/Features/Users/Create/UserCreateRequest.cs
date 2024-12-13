using CourseAI.Application.Core;
using CourseAI.Application.Models;
using FluentValidation;

namespace CourseAI.Application.Features.Users.Create;

public class UserCreateRequest : IRequestModel<UserModel>
{
    public string UserName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }

    public class Validator : AbstractValidator<UserCreateRequest>
    {
        public Validator()
        {
            RuleFor(x => x.UserName).NotEmpty().MinimumLength(1).MaximumLength(100);
            RuleFor(x => x.FirstName).NotEmpty();
            RuleFor(x => x.LastName).NotEmpty();
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
        }
    }
}
