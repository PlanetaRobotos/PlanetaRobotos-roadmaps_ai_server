using CourseAI.Application.Core;
using CourseAI.Application.Models.UserRoadmaps;
using FluentValidation;

namespace CourseAI.Application.Features.Users.UserRoadmaps.Add;

public class UserRoadmapAddRequest : 
    UserRoadmapModelBase, IValidatable<UserRoadmapAddRequest>, IRequestModel<UserRoadmapModel>
{
    public void ConfigureValidator(InlineValidator<UserRoadmapAddRequest> validator)
    {
        ConfigureBaseValidator(validator);
    }
}