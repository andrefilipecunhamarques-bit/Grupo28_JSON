using Microsoft.AspNetCore.Mvc;

namespace Grupo28_JSON.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class AuthController : ControllerBase
{
    private readonly Model _model;
    private readonly ILogger<AuthController> _logger;

    public AuthController(Model model, ILogger<AuthController> logger)
    {
        _model = model;
        _logger = logger;
    }

    [HttpGet("ping")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult Ping()
    {
        _logger.LogDebug("Ping endpoint called.");
        return Ok("pong");
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)
    {
        _logger.LogInformation("Login requested for user '{Username}'.", request?.Username);
        var response = await _model.AuthenticateAsync(request);
        _logger.LogInformation("Login finished for user '{Username}' with code '{Code}'.", request?.Username, response.Code);
        return Ok(response);
    }

    [HttpPost("register")]
    [ProducesResponseType(typeof(RegisterResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(RegisterResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(RegisterResponse), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<RegisterResponse>> Register([FromBody] RegisterRequest request)
    {
        _logger.LogInformation("Register requested for user '{Username}'.", request?.Username);
        var response = await _model.RegisterAsync(request);
        _logger.LogInformation("Register finished for user '{Username}' with code '{Code}'.", request?.Username, response.Code);

        return response.Code switch
        {
            "CREATED" => StatusCode(StatusCodes.Status201Created, response),
            "USERNAME_TAKEN" => Conflict(response),
            _ => BadRequest(response)
        };
    }
}
