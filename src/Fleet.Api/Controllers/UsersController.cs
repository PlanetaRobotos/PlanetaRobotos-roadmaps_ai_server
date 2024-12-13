using Fleet.Api.Core;
using Fleet.Api.Extensions;
using Fleet.Application.Features.Users.Create;
using Fleet.Application.Features.Users.Delete;
using Fleet.Application.Features.Users.GetById;
using Fleet.Application.Features.Users.Update;
using Fleet.Application.Features.Users.UserRoadmaps.Add;
using Fleet.Application.Features.Users.UserRoadmaps.Filter;
using Fleet.Application.Models;
using Fleet.Application.Models.Roadmaps;
using Fleet.Application.Models.UserRoadmaps;
using Microsoft.AspNetCore.Mvc;

namespace Fleet.Api.Controllers;

public class UsersController : V1Controller
{
    [HttpGet("{id:long}")]
    public async Task<ActionResult<UserModel>> Get(long id)
    {
        var response = await Sender.Send(new UserGetByIdRequest { Id = id });
        return response.MatchResponse(user => Ok(user));
    }

    [HttpPost]
    public async Task<ActionResult<UserModel>> Create(UserCreateRequest request)
    {
        var response = await Sender.Send(request);
        return response.MatchResponse(user => CreatedAtAction("Get", new { id = user.Id }, user));
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> Update(long id, UserUpdateRequest request)
    {
        request.Id = id;
        var response = await Sender.Send(request);
        return response.MatchEmptyResponse();
    }

    [HttpDelete("{id:long}")]
    public async Task<ActionResult<UserModel>> Delete(long id)
    {
        var response = await Sender.Send(new UserDeleteRequest { Id = id });
        return response.MatchEmptyResponse();
    }
    
    [HttpGet("{id:long}/roadmaps")]
    public async Task<ActionResult<List<UserRoadmapModel>>> GetUserRoadmaps(long id)
    {
        var response = await Sender.Send(new UserRoadmapFilterRequest { UserId = id });
        return response.MatchResponse(roadmaps => Ok(roadmaps));
    }

    [HttpPost("{id:long}/roadmaps/{roadmapId:guid}")]
    public async Task<ActionResult<UserRoadmapModel>> AddUserRoadmap(long id, Guid roadmapId)
    {
        var response = await Sender.Send(new UserRoadmapAddRequest { UserId = id, RoadmapId = roadmapId });
        return response.MatchResponse(roadmaps => Ok(roadmaps));
    }

    // [HttpDelete("{userId:long}/roadmaps/{roadmapId:long}")]
    // public async Task<IActionResult> RemoveUserRoadmap(long userId, long roadmapId)
    // {
    //     var response = await Sender.Send(new RemoveUserRoadmapRequest { UserId = userId, RoadmapId = roadmapId });
    //     return response.MatchEmptyResponse();
    // }
    
    // [HttpPut("{userId:long}/roadmaps/{roadmapId:long}")]
    // public async Task<IActionResult> UpdateUserRoadmap(long userId, long roadmapId, UpdateUserRoadmapRequest request)
    // {
    //     request.UserId = userId;
    //     request.RoadmapId = roadmapId;
    //     var response = await Sender.Send(request);
    //     return response.MatchEmptyResponse();
    // }
}