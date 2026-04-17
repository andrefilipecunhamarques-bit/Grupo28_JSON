using Microsoft.AspNetCore.Mvc;

namespace Grupo28_JSON.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class AuthController : ControllerBase
{
    [HttpGet("ping")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult Ping()
    {
        return Ok("pong");
    }
}
