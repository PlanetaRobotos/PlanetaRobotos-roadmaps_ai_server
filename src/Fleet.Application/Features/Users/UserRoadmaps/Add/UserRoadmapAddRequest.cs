using Fleet.Application.Core;
using Fleet.Application.Models.UserRoadmaps;
using FluentValidation;

namespace Fleet.Application.Features.Users.UserRoadmaps.Add;

public class UserRoadmapAddRequest : 
    UserRoadmapModelBase, IValidatable<UserRoadmapAddRequest>, IRequestModel<UserRoadmapModel>
{
    public void ConfigureValidator(InlineValidator<UserRoadmapAddRequest> validator)
    {
        ConfigureBaseValidator(validator);
    }
}
