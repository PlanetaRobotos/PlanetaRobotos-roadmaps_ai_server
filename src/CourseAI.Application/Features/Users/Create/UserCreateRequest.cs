using CourseAI.Application.Core;
using CourseAI.Application.Models;
using CourseAI.Domain.Entities.Identity;
using FluentValidation;

namespace CourseAI.Application.Features.Users.Create;

public class UserCreateRequest : UserModelBase, IValidatable<UserCreateRequest>, IRequestModel<UserModel>
{
    public void ConfigureValidator(InlineValidator<UserCreateRequest> validator)
    {
        // validator.RuleFor(x => x.UserName).NotEmpty().MinimumLength(1).MaximumLength(100);
        validator.RuleFor(x => x.Email).NotEmpty().EmailAddress();
    }
}
