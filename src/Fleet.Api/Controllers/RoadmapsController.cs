using Fleet.Api.Core;
using Fleet.Api.Extensions;
using Fleet.Application.Features.Roadmaps.Create;
using Fleet.Application.Features.Roadmaps.Delete;
using Fleet.Application.Features.Roadmaps.Filter;
using Fleet.Application.Features.Roadmaps.GetById;
using Fleet.Application.Features.Roadmaps.Update;
using Fleet.Application.Models.Roadmaps;
using Fleet.Application.Models.Shared;
using Fleet.Application.Models.UserRoadmaps;
using Microsoft.AspNetCore.Mvc;

namespace Fleet.Api.Controllers;

public class RoadmapsController : V1Controller
{
    
    [HttpGet("{id:guid}")]
    [ProducesResponseType<UserRoadmapModel>(StatusCodes.Status200OK)]
    public async Task<ActionResult> GetById(Guid id)
    {
        var response = await Sender.Send(new RoadmapGetByIdRequest { Id = id });
        return response.MatchResponse(roadmap => Ok(roadmap));
    }
    
    [HttpGet]
    [ProducesResponseType<Filtered<UserRoadmapModel>>(StatusCodes.Status200OK)]
    public async Task<ActionResult> Filter([FromQuery] RoadmapFilterRequest request)
    {
        var response = await Sender.Send(request);
        return response.MatchResponse(Roadmaps => Ok(Roadmaps));
    }
    
    [HttpPost]
    [ProducesResponseType<UserRoadmapModel>(StatusCodes.Status201Created)]
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
}
