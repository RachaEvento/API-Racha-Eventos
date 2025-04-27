using OrganizadorEventos.Request;

namespace OrganizadorEventos.Model;

public class Contato
{
    public Contato()
    {
    }

    public Contato(ContatoRequest request, Guid usuarioId)
    {
        Id = Guid.NewGuid();
        UsuarioId = usuarioId;
        Nome = request.Nome;
        Email = request.Email;
        Telefone = request.Telefone;
        Ativo = request.Ativo;
    }

    public Contato(ContatoRequest request, Guid usuarioId, Guid id)
    {
        Id = id;
        UsuarioId = usuarioId;
        Nome = request.Nome;
        Email = request.Email;
        Telefone = request.Telefone;
        Ativo = request.Ativo;
    }

    public Guid Id { get; set; }
    public Guid UsuarioId { get; set; }
    public string Nome { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Telefone { get; set; } = null!;
    public bool Ativo { get; set; }

    public Usuario Usuario { get; set; } = null!;
    public ICollection<Participante> Participantes { get; set; } = new List<Participante>();
}