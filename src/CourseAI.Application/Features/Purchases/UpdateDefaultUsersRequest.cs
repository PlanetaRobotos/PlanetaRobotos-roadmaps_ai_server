using CourseAI.Application.Core;

namespace CourseAI.Application.Features.Purchases;

public class UpdateDefaultUsersRequest: IRequestModel
{
    public string Plan { get; set; }
}