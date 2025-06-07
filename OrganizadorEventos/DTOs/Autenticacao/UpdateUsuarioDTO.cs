using OrganizadorEventos.Enum;

namespace OrganizadorEventos.DTOs.Autenticacao;

public class UpdateUsuarioDTO
{
    public string? Email { get; set; }
    public string? Nome { get; set; }
    public string? Numero { get; set; }
    public string? ChavePix { get; set; }
    public TipoChavePix? TipoChavePix { get; set; }
}