using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using CourseAI.Application.Core;
using FluentValidation;

namespace CourseAI.Application.Features.Users.Update;

public class UserUpdateRequest : IRequestModel
{
    public long Id { get; set; }

    [Required]
    public string UserName { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    public class Validator : AbstractValidator<UserUpdateRequest>
    {
        public Validator()
        {
            RuleFor(x => x.UserName).NotEmpty().MinimumLength(1).MaximumLength(100);
            // RuleFor(x => x.FirstName).NotEmpty();
            // RuleFor(x => x.LastName).NotEmpty();
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
        }
    }
}
