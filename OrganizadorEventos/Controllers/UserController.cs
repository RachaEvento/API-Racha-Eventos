using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OrganizadorEventos.Enum;
using OrganizadorEventos.Interfaces;
using OrganizadorEventos.Model;
using OrganizadorEventos.Request;

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
            return BadRequest("Usuário não pode ser nulo.");

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
                return BadRequest(string.Join(", ", result.Errors.Select(e => e.Description)));

            await _userManager.AddToRoleAsync(user, rolePadrao);

            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }


    [HttpGet("getUser/{id}")]
    public async Task<IActionResult> GetUserById(string id)
    {
        var user = await _userRepository.GetUserByIdAsync(id);

        if (user == null)
            return NotFound();

        return Ok(user);
    }
}