using CourseAI.Application.Core;
using CourseAI.Application.Models.UserLikes;
using FluentValidation;

namespace CourseAI.Application.Features.Roadmaps.UserLikes;

public class UserLikeAddRequest : 
    UserLikeModelBase, IValidatable<UserLikeAddRequest>, IRequestModel<UserLikeModel>
{
    public void ConfigureValidator(InlineValidator<UserLikeAddRequest> validator)
    {
        ConfigureBaseValidator(validator);
    }
}