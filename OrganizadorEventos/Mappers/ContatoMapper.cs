using OrganizadorEventos.DTOs.Contato;
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
            FotoBase64 = entity.Foto != null ? Convert.ToBase64String(entity.Foto) : null
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
            UsuarioId = usuarioId
        };
    }
    public static Contato ToEntity(this ContatoCreateDTO dto, Guid usuarioId, byte[] fotoBytes)
    {
        return new Contato
        {
            Id = Guid.NewGuid(),
            Nome = dto.Nome,
            Email = dto.Email,
            Telefone = dto.Telefone,
            Ativo = dto.Ativo,
            UsuarioId = usuarioId,
            Foto = fotoBytes // seu campo bytea na tabela
        };
    }
    public static Contato ToEntity(this ContatoUpdateDTO dto, Guid userId, byte[]? fotoBytes = null)
    {
        return new Contato
        {
            Id = dto.Id,
            Nome = dto.Nome,
            Email = dto.Email,
            Telefone = dto.Telefone,
            Foto = fotoBytes, 
            UsuarioId = userId
        };
    }
}