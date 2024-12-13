using System.Text.Json.Serialization;
using Fleet.Application.Core;
using FluentValidation;

namespace Fleet.Application.Features.Users.Update;

public class UserUpdateRequest : IRequestModel
{
    [JsonIgnore]
    public long Id { get; set; }

    public string UserName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }

    public class Validator : AbstractValidator<UserUpdateRequest>
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
