using OrganizadorEventos.Model;

namespace OrganizadorEventos.Request;

public class ContatoRequest
{
    public Guid? Id { get; set; }
    public string Nome { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Telefone { get; set; } = null!;
    public bool Ativo { get; set; }
}