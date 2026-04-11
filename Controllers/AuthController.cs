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

    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status503ServiceUnavailable)]
    public ActionResult<LoginResponse> Login([FromBody] LoginRequest request)
    {
        var resposta = model.Autenticar(request);

        if (resposta.Code == "DB_OFFLINE")
        {
            return StatusCode(StatusCodes.Status503ServiceUnavailable, resposta);
        }

        return Ok(resposta);
    }
}
