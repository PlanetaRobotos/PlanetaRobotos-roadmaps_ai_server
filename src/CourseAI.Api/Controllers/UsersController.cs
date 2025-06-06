using System.Security.Claims;
using CourseAI.Api.Core;
using CourseAI.Api.Extensions;
using CourseAI.Application.Features.Users.Create;
using CourseAI.Application.Features.Users.Delete;
using CourseAI.Application.Features.Users.GetById;
using CourseAI.Application.Features.Users.Update;
using CourseAI.Application.Features.Users.UserQuizzes.Filter;
using CourseAI.Application.Features.Users.UserQuizzes.Update;
using CourseAI.Application.Features.Users.UserRoadmaps.Add;
using CourseAI.Application.Features.Users.UserRoadmaps.Delete;
using CourseAI.Application.Features.Users.UserRoadmaps.Filter;
using CourseAI.Application.Features.Users.UserRoadmaps.GetById;
using CourseAI.Application.Models;
using CourseAI.Application.Models.Roadmaps;
using CourseAI.Application.Models.Shared;
using CourseAI.Application.Models.UserRoadmaps;
using CourseAI.Application.Services;
using CourseAI.Domain.Entities.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CourseAI.Api.Controllers;

public class UsersController(UserManager<User> userManager, IUserService userService) : V1Controller
{
    [Authorize]
    [HttpGet("me")]
    [ProducesResponseType(typeof(UserModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetCurrentUser()
    {
        var userResult = await userService.GetUser();
        
        return userResult.Match(
            Ok,
            error => throw new Exception(error.Message)
        );
    }

    [HttpGet("{id:long}")]
    [ProducesResponseType<UserModel>(StatusCodes.Status200OK)]
    public async Task<ActionResult<UserModel>> Get(long id)
    {
        var response = await Sender.Send(new UserGetByIdRequest { Id = id });
        return response.MatchResponse(user => Ok(user));
    }

    [HttpPost]
    [ProducesResponseType<UserModel>(StatusCodes.Status201Created)]
    public async Task<ActionResult<UserModel>> Create(UserCreateRequest request)
    {
        var response = await Sender.Send(request);
        return response.MatchResponse(user => CreatedAtAction("Get", new { id = user.Id }, user));
    }

    [HttpPut("{id:long}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Update(long id, UserUpdateRequest request)
    {
        request.Id = id;
        var response = await Sender.Send(request);
        return response.MatchEmptyResponse();
    }

    [HttpDelete("{id:long}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult<UserModel>> Delete(long id)
    {
        var response = await Sender.Send(new UserDeleteRequest { Id = id });
        return response.MatchEmptyResponse();
    }

    [HttpGet("{userId:long}/roadmaps")]
    [ProducesResponseType<Filtered<UserRoadmapModel>>(StatusCodes.Status200OK)]
    public async Task<ActionResult> GetUserRoadmaps(long userId, [FromQuery] UserRoadmapFilterRequest request)
    {
        request.UserId = userId;
        var response = await Sender.Send(request);
        return response.MatchResponse(UserRoadmaps => Ok(UserRoadmaps));
    }
    
    // [HttpGet("{userId:long}/roadmaps")]
    // [ProducesResponseType<Filtered<UserRoadmapModel>>(StatusCodes.Status200OK)]
    // public async Task<ActionResult> GetSavedRoadmaps(long userId, [FromQuery] UserRoadmapFilterRequest request)
    // {
    //     request.UserId = userId;
    //     var response = await Sender.Send(request);
    //     return response.MatchResponse(UserRoadmaps => Ok(UserRoadmaps));
    // }

    [HttpGet("{userId:long}/roadmaps/{roadmapId:guid}")]
    [ProducesResponseType<UserRoadmapModel>(StatusCodes.Status200OK)]
    public async Task<ActionResult> GetUserRoadmapById(long userId, Guid roadmapId)
    {
        var response = await Sender.Send(new UserRoadmapGetByIdRequest { UserId = userId, RoadmapId = roadmapId });
        return response.MatchResponse(roadmaps => Ok(roadmaps));
    }

    [HttpPost("{userId:long}/roadmaps/{roadmapId:guid}")]
    [ProducesResponseType<UserRoadmapModel>(StatusCodes.Status201Created)]
    public async Task<ActionResult> AddUserRoadmap(long userId, Guid roadmapId)
    {
        var response = await Sender.Send(new UserRoadmapAddRequest { UserId = userId, RoadmapId = roadmapId });
        return response.MatchResponse(userRoadmap => CreatedAtAction(nameof(GetUserRoadmapById), new { userId = userRoadmap.UserId, roadmapId = userRoadmap.RoadmapId }, userRoadmap));
    }
    
    [HttpDelete("{userId:long}/roadmaps/{roadmapId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> Delete(long userId, Guid roadmapId)
    {
        var response = await Sender.Send(new UserRoadmapDeleteRequest { UserId = userId, RoadmapId = roadmapId });
        return response.MatchEmptyResponse();
    }

    [HttpPost("{userId:long}/quizzes/{quizId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdateUserQuiz(long userId, Guid quizId, UserQuizResultUpdateRequest request)
    {
        request.UserId = userId;
        request.QuizId = quizId;
        var response = await Sender.Send(request);
        return response.MatchEmptyResponse();
    }

    [HttpGet("{userId:long}/quizzes")]
    [ProducesResponseType<Filtered<UserQuizModel>>(StatusCodes.Status200OK)]
    public async Task<ActionResult> GetUserQuizzes(long userId, [FromQuery] UserQuizFilterRequest request)
    {
        request.UserId = userId;
        var response = await Sender.Send(request);
        return response.MatchResponse(UserQuizzes => Ok(UserQuizzes));
    }
    
    [HttpGet("roles")]
    [Authorize]
    public async Task<IActionResult> GetUserRoles()
    {
        try
        {
            var userResult = await userService.GetUser();
            var user = userResult.Match(
                user => user,
                error => throw new Exception(error.Message)
            );

            var roles = await userManager.GetRolesAsync(user);
            return Ok(new { Roles = roles });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}
