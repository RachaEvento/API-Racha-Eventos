using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using OrganizadorEventos.DTOs.Autenticacao;
using OrganizadorEventos.Enum;
using OrganizadorEventos.Interfaces.Services;
using OrganizadorEventos.Model;
using OrganizadorEventos.Request.Autenticacao;

namespace OrganizadorEventos.Services;

public class AuthService : IAuthService
{
    private readonly IConfiguration _configuration;
    private readonly UserManager<Usuario> _userManager;

    public AuthService(UserManager<Usuario> userManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _configuration = configuration;
    }

    public async Task<string> LoginAsync(LoginDTO loginDto)
    {
        var user = await _userManager.FindByEmailAsync(loginDto.Email);
        if (user == null)
            throw new ArgumentException("Usuário não encontrado.");

        var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);
        
        if (!result)
            throw new ArgumentException("Email ou Senha incorreta.");

        var roles = await _userManager.GetRolesAsync(user);

        return GenerateJwtToken(user, roles);
    }

    public async Task<string> RegisterAsync(RegisterDTO registerDto)
    {
        var existingUser = await _userManager.FindByEmailAsync(registerDto.Email);
        if (existingUser != null)
            throw new ArgumentException("Já existe um usuário com este email.");
        
        if (!string.IsNullOrWhiteSpace(registerDto.ChavePix) && registerDto.TipoChavePix == null)
        {
            throw new ArgumentException("Tipo da chave Pix deve ser informado quando uma Chave Pix for fornecida.");
        }
        var user = new Usuario
        {
            UserName = registerDto.Nome,
            Email = registerDto.Email,
            PhoneNumber = registerDto.Numero,
            ChavePix = registerDto.ChavePix,
            TipoChavePix = registerDto.TipoChavePix,
        };

        var result = await _userManager.CreateAsync(user, registerDto.Password);

        if (!result.Succeeded)
        {
            var erros = result.Errors.Select(e => e.Description).ToList();
            throw new ArgumentException(string.Join("; ", erros));
        }

        await _userManager.AddToRoleAsync(user, UserRole.Usuario.ToString());
        
        return "Usuário registrado com sucesso. Faça login para continuar.";
    }
    public async Task AtualizarUsuarioAsync(Guid userId, UpdateUsuarioDTO dto)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
            throw new ArgumentException("Usuário não encontrado.");

        if (!string.IsNullOrWhiteSpace(dto.Nome))
            user.UserName = dto.Nome;

        if (!string.IsNullOrWhiteSpace(dto.Numero))
            user.PhoneNumber = dto.Numero;

        user.ChavePix = dto.ChavePix;

        if (!string.IsNullOrWhiteSpace(dto.ChavePix) && dto.TipoChavePix == null)
            throw new ArgumentException("Tipo da chave Pix deve ser informado quando uma Chave Pix for fornecida.");

        user.TipoChavePix = dto.TipoChavePix;

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            var erros = result.Errors.Select(e => e.Description).ToList();
            throw new ArgumentException(string.Join("; ", erros));
        }
    }

    // Método que gera o token JWT
    private string GenerateJwtToken(Usuario usuario, IList<string> roles)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Name, usuario.UserName),
            new(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()),
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