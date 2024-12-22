using CourseAI.Api.Core;
using CourseAI.Api.Extensions;
using CourseAI.Application.Features.Roadmaps.Create;
using CourseAI.Application.Features.Roadmaps.Delete;
using CourseAI.Application.Features.Roadmaps.Filter;
using CourseAI.Application.Features.Roadmaps.GetById;
using CourseAI.Application.Features.Roadmaps.Update;
using CourseAI.Application.Features.Roadmaps.UserLikes;
using CourseAI.Application.Features.Users.UserRoadmaps.GetById;
using CourseAI.Application.Models.Roadmaps;
using CourseAI.Application.Models.Shared;
using CourseAI.Application.Models.UserLikes;
using CourseAI.Application.Models.UserRoadmaps;
using Microsoft.AspNetCore.Mvc;

namespace CourseAI.Api.Controllers;

public class RoadmapsController : V1Controller
{
    
    [HttpGet("{id:guid}")]
    [ProducesResponseType<RoadmapModel>(StatusCodes.Status200OK)]
    public async Task<ActionResult> GetById(Guid id)
    {
        var response = await Sender.Send(new RoadmapGetByIdRequest { Id = id });
        return response.MatchResponse(roadmap => Ok(roadmap));
    }
    
    [HttpGet]
    [ProducesResponseType<Filtered<RoadmapModel>>(StatusCodes.Status200OK)]
    public async Task<ActionResult> Filter([FromQuery] RoadmapFilterRequest request)
    {
        var response = await Sender.Send(request);
        return response.MatchResponse(Roadmaps => Ok(Roadmaps));
    }
    
    [HttpPost]
    [ProducesResponseType<RoadmapModel>(StatusCodes.Status201Created)]
    public async Task<ActionResult> Create(RoadmapCreateRequest request)
    {
        var response = await Sender.Send(request);
        return response.MatchResponse(roadmap => CreatedAtAction(nameof(GetById), new { id = roadmap.Id }, roadmap));
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Update(Guid id, RoadmapUpdateRequest request)
    {
        request.Id = id;
        var response = await Sender.Send(request);
        return response.MatchEmptyResponse();
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> Delete(Guid id)
    {
        var response = await Sender.Send(new RoadmapDeleteRequest { Id = id });
        return response.MatchEmptyResponse();
    }
    
    [HttpGet("{roadmapId:guid}/userlikes/{userId:long}")]
    [ProducesResponseType<UserLikeModel>(StatusCodes.Status200OK)]
    public async Task<ActionResult> GetUserLikeById(long userId, Guid roadmapId)
    {
        var response = await Sender.Send(new UserLikeGetByIdRequest { UserId = userId, RoadmapId = roadmapId });
        return response.MatchResponse(roadmaps =>
        {
            return Ok(roadmaps);
        });
    }

    [HttpPost("userlikes")]
    [ProducesResponseType<UserLikeModel>(StatusCodes.Status201Created)]
    public async Task<ActionResult> AddUserLike(UserLikeAddRequest request)
    {
        var response = await Sender.Send(request);
        return response.MatchResponse(UserLike => CreatedAtAction(nameof(GetUserLikeById), new { userId = UserLike.UserId, roadmapId = UserLike.RoadmapId }, UserLike));
    }
    
    [HttpDelete("{roadmapId:guid}/userlikes/{userId:long}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> Delete(long userId, Guid roadmapId)
    {
        var response = await Sender.Send(new UserLikeDeleteRequest { UserId = userId, RoadmapId = roadmapId });
        return response.MatchEmptyResponse();
    }
}
