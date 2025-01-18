// using CourseAI.Api.Core;
// using CourseAI.Application.Models.Roadmaps;
// using CourseAI.Application.Models.Shared;
// using Mediator;
// using Microsoft.AspNetCore.Mvc;
//
// namespace CourseAI.Api.Controllers;
//
// public class RoomsController(IMediator mediator) : V1Controller
// {
//     [HttpGet("room/{category}")]
//     [ProducesResponseType<Filtered<RoadmapModel>>(StatusCodes.Status200OK)]
//     public async Task<IActionResult> GetByCategory([FromRoute] string category, [FromQuery] RoomRequest request)
//     {
//         var response = await Sender.Send(request with { Category = category });
//         return response.MatchResponse(roadmaps => Ok(roadmaps));
//     }
// }