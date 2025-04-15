using OrganizadorEventos.Request;

namespace OrganizadorEventos.Interfaces.Services;

public interface IAuthService
{
    Task<string> LoginAsync(LoginRequest loginRequest);
    Task<string> RegisterAsync(RegisterRequest registerRequest);
}