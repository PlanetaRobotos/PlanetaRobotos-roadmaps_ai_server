using CourseAI.Application.Core;

namespace CourseAI.Application.Features.Purchases;

public class DeletePlanRequest: IRequestModel
{
    public string Plan { get; set; }
}