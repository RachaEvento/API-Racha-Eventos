using OrganizadorEventos.Request.Autenticacao;

namespace OrganizadorEventos.Interfaces.Services;

public interface IAuthService
{
    Task<string> LoginAsync(LoginDTO loginDto);
    Task<string> RegisterAsync(RegisterDTO registerDto);
}