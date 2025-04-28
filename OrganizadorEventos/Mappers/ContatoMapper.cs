using OrganizadorEventos.Model;
using OrganizadorEventos.Request;

namespace OrganizadorEventos.Mappers;

public static class ContatoMapper
{
    public static ContatoDTO ToRequest(this Contato entity)
    {
        return new ContatoDTO
        {
            Id = entity.Id,
            Nome = entity.Nome,
            Email = entity.Email,
            Telefone = entity.Telefone,
            Ativo = entity.Ativo,
        };
    }

    public static Contato ToEntity(this ContatoDTO dto, Guid usuarioId)
    {
        return new Contato
        {
            Id = dto.Id ?? Guid.NewGuid(),
            Nome = dto.Nome,
            Email = dto.Email,
            Telefone = dto.Telefone,
            Ativo = dto.Ativo,
            UsuarioId =  usuarioId
        };
    }
}