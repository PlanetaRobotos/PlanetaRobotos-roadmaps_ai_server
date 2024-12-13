using System.Reflection;
using Fleet.Api.Core;
using Fleet.Application.Models;
using Microsoft.AspNetCore.Mvc;

namespace Fleet.Api.Controllers;

[Tags("Server Info")]
public class ServerInfoController : V1Controller
{
    [HttpGet]
    [ProducesResponseType<ServerInfoModel>(StatusCodes.Status200OK)]
    public IActionResult GetInfo()
    {
        return Ok(new ServerInfoModel
        {
            Name = "Fleet",
            ApiVersion = "1.0",
            BuildId = Assembly.GetExecutingAssembly().ManifestModule.ModuleVersionId.ToString("N")
        });
    }
}