using Fleet.Api.Core;
using Fleet.Api.Extensions;
using Fleet.Application.Features.Lessons.Create;
using Fleet.Application.Features.Lessons.Delete;
using Fleet.Application.Features.Lessons.Filter;
using Fleet.Application.Features.Lessons.GetById;
using Fleet.Application.Features.Lessons.Update;
using Fleet.Application.Models.Roadmaps;
using Fleet.Application.Models.Shared;
using Microsoft.AspNetCore.Mvc;

namespace Fleet.Api.Controllers;

public class LessonsController : V1Controller
{
    
    [HttpGet("{id:guid}")]
    [ProducesResponseType<LessonModel>(StatusCodes.Status200OK)]
    public async Task<ActionResult> GetById(Guid id)
    {
        var response = await Sender.Send(new LessonGetByIdRequest { Id = id });
        return response.MatchResponse(Lesson => Ok(Lesson));
    }
    
    [HttpGet]
    [ProducesResponseType<Filtered<LessonModel>>(StatusCodes.Status200OK)]
    public async Task<ActionResult> Filter([FromQuery] LessonFilterRequest request)
    {
        var response = await Sender.Send(request);
        return response.MatchResponse(Lessons => Ok(Lessons));
    }
    
    [HttpPost]
    [ProducesResponseType<LessonModel>(StatusCodes.Status201Created)]
    public async Task<ActionResult> GeneratedPage(LessonCreateRequest request)
    {
        var response = await Sender.Send(request);
        return response.MatchResponse(Lesson => CreatedAtAction(nameof(GetById), new { id = Lesson.Id }, Lesson));
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Update(Guid id, LessonUpdateRequest request)
    {
        request.Id = id;
        var response = await Sender.Send(request);
        return response.MatchEmptyResponse();
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> Delete(Guid id)
    {
        var response = await Sender.Send(new LessonDeleteRequest { Id = id });
        return response.MatchEmptyResponse();
    }
}
