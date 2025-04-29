using OrganizadorEventos.Model;
using OrganizadorEventos.Request.Local;

namespace OrganizadorEventos.Mappers;

public static class LocalMapper
{
    public static LocalRequest ToRequest(this Local entity)
    {
        return new LocalRequest
        {
            Id = entity.Id,
            DescricaoLocal = entity.DescricaoLocal,
            Nome = entity.Nome,
            Endereco = entity.Endereco,
            Bairro = entity.Bairro,
            Cidade = entity.Cidade,
            Estado = entity.Estado,
            Ativo = entity.Ativo
        };
    }

    public static Local ToEntity(this LocalRequest request, Guid usuarioId)
    {
        return new Local
        {
            Id = request.Id ?? Guid.NewGuid(),
            DescricaoLocal = request.DescricaoLocal,
            Nome = request.Nome,
            Endereco = request.Endereco,
            Bairro = request.Bairro,
            Cidade = request.Cidade,
            Estado = request.Estado,
            Ativo = request.Ativo,
            UsuarioId = usuarioId
        };
    }
}