using Microsoft.AspNetCore.Mvc;

namespace Grupo28_JSON.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class AuthController : ControllerBase
{
    private readonly Model model;

    public AuthController(Model model)
    {
        this.model = model;
    }

    [HttpGet("ping")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult Ping()
    {
        return Ok("pong");
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status503ServiceUnavailable)]
    public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)
    {
        var resposta = await model.AutenticarAsync(request);

        // 503 reflete indisponibilidade do ator externo, nao logica de negocio interna.
        if (resposta.Code == "DB_OFFLINE")
        {
            return StatusCode(StatusCodes.Status503ServiceUnavailable, resposta);
        }

        return Ok(resposta);
    }
}
