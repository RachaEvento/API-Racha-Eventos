using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OrganizadorEventos.Enum;
using OrganizadorEventos.Interfaces;
using OrganizadorEventos.Model;
using OrganizadorEventos.Request;
using OrganizadorEventos.Response;

namespace OrganizadorEventos.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;
    private readonly IUserRepository _userRepository;

    public UserController(IUserRepository userRepository, UserManager<User> userManager,
        SignInManager<User> signInManager)
    {
        _userRepository = userRepository;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [HttpPost("createUser")]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
    {
        if (request == null)
            return BadRequest(GenericResponse<string>.ErroResponse(new List<string> { "Usuário não pode ser nulo." }));

        try
        {
            var rolePadrao = string.IsNullOrEmpty(request.Role) ? UserRole.Usuario.ToString() : request.Role;

            var user = new User
            {
                Email = request.Email,
                Nome = request.Nome,
                Numero = request.Numero,
                UserName = request.Email
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                var erros = result.Errors.Select(e => e.Description).ToList();
                return BadRequest(GenericResponse<string>.ErroResponse(erros, "Erro ao criar usuário."));
            }

            await _userManager.AddToRoleAsync(user, rolePadrao);

            return CreatedAtAction(
                nameof(GetUserById),
                new { id = user.Id },
                GenericResponse<User>.SucessoResponse(user, "Usuário criado com sucesso.")
            );
        }
        catch (ArgumentException ex)
        {
            return BadRequest(GenericResponse<string>.ErroResponse(new List<string> { ex.Message }));
        }
    }

    [HttpGet("getUser/{id}")]
    public async Task<IActionResult> GetUserById(string id)
    {
        var user = await _userRepository.GetUserByIdAsync(id);

        if (user == null)
            return NotFound(GenericResponse<string>.ErroResponse(new List<string> { "Usuário não encontrado." }));

        return Ok(GenericResponse<User>.SucessoResponse(user, "Usuário encontrado com sucesso."));
    }
}