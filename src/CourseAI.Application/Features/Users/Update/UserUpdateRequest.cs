using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using CourseAI.Application.Core;
using FluentValidation;

namespace CourseAI.Application.Features.Users.Update;

public class UserUpdateRequest : IRequestModel
{
    public long Id { get; set; }

    public string? Name { get; set; }
    
    [MaxLength(500)]
    public string? Bio { get; set; }

    public class Validator : AbstractValidator<UserUpdateRequest>
    {
        public Validator()
        {
            RuleFor(x => x.Name).MaximumLength(100);
            RuleFor(x => x.Bio).MaximumLength(500);
        }
    }
}