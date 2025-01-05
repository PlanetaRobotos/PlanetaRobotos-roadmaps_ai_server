using CourseAI.Api.Core;
using CourseAI.Api.Extensions;
using CourseAI.Application.Features.Purchases;
using CourseAI.Core.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseAI.Api.Controllers;

public class PurchaseController : V1Controller
{
    [HttpPost("buy-plan")]
    [Authorize]
    public async Task<IActionResult> BuyPlan([FromBody] BuyPlanRequest request)
    {
        var response = await Sender.Send(request);
        return response.MatchEmptyResponse();
    }
    
    [HttpDelete("delete-plan")]
    [Authorize]
    public async Task<IActionResult> DeletePlan([FromBody] DeletePlanRequest request)
    {
        var response = await Sender.Send(request);
        return response.MatchEmptyResponse();
    }
    
    [HttpPost("update-default-users")]
    public async Task<IActionResult> UpdateDefaultUsers([FromBody] UpdateDefaultUsersRequest request)
    {
        var response = await Sender.Send(request);
        return response.MatchEmptyResponse();
    }
}