using Microsoft.AspNetCore.Mvc;

namespace Grupo28_JSON.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class AuthController : ControllerBase
{
    private readonly Model _model;

    public AuthController(Model model)
    {
        _model = model;
    }

    [HttpGet("ping")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult Ping()
    {
        return Ok("pong");
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)
    {
        var response = await _model.AuthenticateAsync(request);
        return Ok(response);
    }

    [HttpPost("register")]
    [ProducesResponseType(typeof(RegisterResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(RegisterResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(RegisterResponse), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<RegisterResponse>> Register([FromBody] RegisterRequest request)
    {
        var response = await _model.RegisterAsync(request);

        return response.Code switch
        {
            "CREATED" => StatusCode(StatusCodes.Status201Created, response),
            "USERNAME_TAKEN" => Conflict(response),
            _ => BadRequest(response)
        };
    }
}
