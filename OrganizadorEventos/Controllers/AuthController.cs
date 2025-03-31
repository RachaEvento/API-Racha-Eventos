using Microsoft.AspNetCore.Mvc;
using OrganizadorEventos.Interfaces;
using OrganizadorEventos.Request;

namespace OrganizadorEventos.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
    {
        if (loginRequest == null)
            return BadRequest("Dados de login não fornecidos.");

        try
        {
            var token = await _authService.LoginAsync(loginRequest);
            return Ok(new { Token = token });
        }
        catch (ArgumentException ex)
        {
            return Unauthorized(new { ex.Message });
        }
    }
}