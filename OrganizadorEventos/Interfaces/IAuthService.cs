using OrganizadorEventos.Request;

namespace OrganizadorEventos.Interfaces;

public interface IAuthService
{
    Task<string> LoginAsync(LoginRequest loginRequest);
    Task<string> RegisterUserAsync(CreateUserRequest createUserRequest);
}