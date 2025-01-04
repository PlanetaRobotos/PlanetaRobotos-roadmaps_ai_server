using CourseAI.Api.Core;
using CourseAI.Api.Extensions;
using CourseAI.Application.Features.Tokens;
using CourseAI.Application.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseAI.Api.Controllers;

public class TokensController: V1Controller
{
    [HttpGet("balance")]
    [Authorize]
    [ProducesResponseType<UserModel>(StatusCodes.Status200OK)]
    public async Task<ActionResult<int>> GetBalance()
    {
        var response = await Sender.Send(new TokensGetBalanceRequest());
        return response.MatchResponse(Tokens => Ok(Tokens));
    }
    
    [HttpPost("refill")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> RefillTokens(RefillTokensRequest request)
    {
        var response = await Sender.Send(request);
        return response.MatchEmptyResponse();
    }
}