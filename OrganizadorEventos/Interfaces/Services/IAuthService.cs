using OrganizadorEventos.DTOs.Autenticacao;
using OrganizadorEventos.Request.Autenticacao;

namespace OrganizadorEventos.Interfaces.Services;

public interface IAuthService
{
    Task<string> LoginAsync(LoginDTO loginDto);
    Task<string> RegisterAsync(RegisterDTO registerDto);
    Task AtualizarUsuarioAsync(Guid userId, UpdateUsuarioDTO dto);
    Task<UsuarioDTO> ObterUsuarioPorIdAsync(Guid userId);
}