using Microsoft.AspNetCore.Mvc;
using OrganizadorEventos.Interfaces.Services;
using OrganizadorEventos.Request.Autenticacao;
using OrganizadorEventos.Response;

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
    public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
    {
        if (loginDto == null)
            return BadRequest(
                GenericResponse<string>.ErroResponse(new List<string> { "Dados de login não fornecidos." }));

        try
        {
            var token = await _authService.LoginAsync(loginDto);
            return Ok(GenericResponse<string>.SucessoResponse(token, "Login realizado com sucesso."));
        }
        catch (ArgumentException ex)
        {
            return Unauthorized(
                GenericResponse<string>.ErroResponse(new List<string> { ex.Message }, "Erro ao fazer login."));
        }
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser([FromBody] RegisterDTO registerDto)
    {
        if (registerDto == null)
            return BadRequest(GenericResponse<string>.ErroResponse(new List<string>
                { "Dados do usuário não fornecidos." }));

        try
        {
            var token = await _authService.RegisterAsync(registerDto);
            return Ok(GenericResponse<string>.SucessoResponse(token, "Usuário registrado com sucesso."));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(GenericResponse<string>.ErroResponse(new List<string> { ex.Message },
                "Erro ao registrar usuário."));
        }
    }
}