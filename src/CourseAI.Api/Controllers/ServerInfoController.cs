using System.Reflection;
using CourseAI.Api.Core;
using CourseAI.Application.Models;
using Microsoft.AspNetCore.Mvc;

namespace CourseAI.Api.Controllers;

[Tags("Server Info")]
public class ServerInfoController : V1Controller
{
    [HttpGet]
    [ProducesResponseType<ServerInfoModel>(StatusCodes.Status200OK)]
    public IActionResult GetInfo()
    {
        return Ok(new ServerInfoModel
        {
            Name = "CourseAI",
            ApiVersion = "1.0",
            BuildId = Assembly.GetExecutingAssembly().ManifestModule.ModuleVersionId.ToString("N")
        });
    }
}