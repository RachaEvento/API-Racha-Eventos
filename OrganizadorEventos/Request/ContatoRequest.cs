using OrganizadorEventos.Model;

namespace OrganizadorEventos.Request;

public class ContatoRequest
{
    public Guid? Id { get; set; }
    public string Nome { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Telefone { get; set; } = null!;
    public bool Ativo { get; set; }

    public ContatoRequest()
    {
        
    }

    public ContatoRequest(Contato entity)
    {
        Id = entity.Id;
        Nome = entity.Nome;
        Email = entity.Email;
        Telefone = entity.Telefone;
        Ativo = entity.Ativo;
    }
}