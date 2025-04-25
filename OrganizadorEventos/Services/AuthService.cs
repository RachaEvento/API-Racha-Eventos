using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using OrganizadorEventos.Enum;
using OrganizadorEventos.Interfaces.Services;
using OrganizadorEventos.Model;
using OrganizadorEventos.Request;

namespace OrganizadorEventos.Services;

public class AuthService : IAuthService
{
    private readonly IConfiguration _configuration;
    private readonly UserManager<ApplicationUser> _userManager;

    public AuthService(UserManager<ApplicationUser> userManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _configuration = configuration;
    }

    public async Task<string> LoginAsync(LoginRequest loginRequest)
    {
        var user = await _userManager.FindByEmailAsync(loginRequest.Email);
        if (user == null)
            throw new ArgumentException("Usuário não encontrado.");

        var result = await _userManager.CheckPasswordAsync(user, loginRequest.Password);

        //Mesmo verificando apenas a senha eu informo que ambos estão errados.
        if (!result)
            throw new ArgumentException("Email ou Senha incorreta.");

        var roles = await _userManager.GetRolesAsync(user);

        return GenerateJwtToken(user, roles);
    }

    public async Task<string> RegisterAsync(RegisterRequest registerRequest)
    {
        var existingUser = await _userManager.FindByEmailAsync(registerRequest.Email);
        if (existingUser != null)
            throw new ArgumentException("Já existe um usuário com este email.");

        var user = new ApplicationUser
        {
            UserName = registerRequest.Nome,
            Email = registerRequest.Email,
            PhoneNumber = registerRequest.Numero
        };

        var result = await _userManager.CreateAsync(user, registerRequest.Password);

        if (!result.Succeeded)
        {
            var erros = result.Errors.Select(e => e.Description).ToList();
            throw new ArgumentException(string.Join("; ", erros));
        }

        await _userManager.AddToRoleAsync(user, UserRole.Usuario.ToString());

        var roles = await _userManager.GetRolesAsync(user);
        return GenerateJwtToken(user, roles);
    }

    // Método que gera o token JWT
    private string GenerateJwtToken(ApplicationUser usuario, IList<string> roles)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, usuario.Id),
            new(JwtRegisteredClaimNames.Email, usuario.Email),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        foreach (var role in roles) claims.Add(new Claim(ClaimTypes.Role, role));

        var token = new JwtSecurityToken(
            _configuration["Jwt:Issuer"],
            _configuration["Jwt:Audience"],
            claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}