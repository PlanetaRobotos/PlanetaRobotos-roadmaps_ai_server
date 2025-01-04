using CourseAI.Application.Core;

namespace CourseAI.Application.Features.Purchases;

public class BuyPlanRequest: IRequestModel
{
    public string Plan { get; set; }
}