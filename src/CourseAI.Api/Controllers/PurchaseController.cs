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
    
    [HttpGet("feature-global")]
    [Authorize]
    public IActionResult FeatureGlobal()
    {
        return Ok("This is a feature for Standard users.");
    }

    [HttpGet("feature-standard")]
    [Authorize(Roles = Roles.Standard)]
    public IActionResult FeatureStandard()
    {
        return Ok("This is a feature for Standard users.");
    }

    [HttpGet("feature-paid-both")]
    [Authorize(Roles = $"{Roles.Standard},{Roles.Enterprise}")]
    public IActionResult FeaturePaidBoth()
    {
        return Ok("This is a feature for Standard and Enterprise users.");
    }
    
    [HttpGet("feature-enterprise")]
    [Authorize(Roles = Roles.Enterprise)]
    public IActionResult FeatureEnterprise()
    {
        return Ok("This is a feature for Enterprise users.");
    }
    
    [HttpGet("feature-admin")]
    [Authorize(Roles = Roles.Admin)]
    public IActionResult FeatureAdmin()
    {
        return Ok("This is a feature for Admin users.");
    }
    
    [HttpGet("feature-user")]
    [Authorize(Roles = Roles.User)]
    public IActionResult FeatureUser()
    {
        return Ok("This is a feature for User users.");
    }
}