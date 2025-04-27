using OrganizadorEventos.Model;
using OrganizadorEventos.Request;

namespace OrganizadorEventos.Mappers;

public static class ContatoMapper
{
    public static ContatoRequest ToRequest(this Contato entity)
    {
        return new ContatoRequest
        {
            Id = entity.Id,
            Nome = entity.Nome,
            Email = entity.Email,
            Telefone = entity.Telefone,
            Ativo = entity.Ativo,
        };
    }

    public static Contato ToEntity(this ContatoRequest request, Guid usuarioId)
    {
        return new Contato
        {
            Id = request.Id ?? Guid.NewGuid(),
            Nome = request.Nome,
            Email = request.Email,
            Telefone = request.Telefone,
            Ativo = request.Ativo,
            UsuarioId =  usuarioId
        };
    }

    public static void UpdateWithRequest(this Contato entity, ContatoRequest request)
    {
        entity.Nome = request.Nome;
        entity.Email = request.Email;
        entity.Telefone = request.Telefone;
        entity.Ativo = request.Ativo;
    }
}