using OrganizadorEventos.Model;
using OrganizadorEventos.Request;

namespace OrganizadorEventos.Mappers;

public static class LocalMapper
{
    public static LocalRequest ToLocalRequest(Local local)
    {
        return new LocalRequest
        {
            LocalId = local.LocalId,
            DescricaoLocal = local.DescricaoLocal,
            Nome = local.Nome,
            Endereco = local.Endereco,
            Bairro = local.Bairro,
            Cidade = local.Cidade,
            Estado = local.Estado,
            Ativo = local.Ativo,
            UsuarioId = local.UsuarioId
        };
    }

    // Mapeia de LocalRequest para Local
    public static Local ToLocalEntity(LocalRequest localRequest)
    {
        return new Local
        {
            LocalId = localRequest.LocalId,
            DescricaoLocal = localRequest.DescricaoLocal,
            Nome = localRequest.Nome,
            Endereco = localRequest.Endereco,
            Bairro = localRequest.Bairro,
            Cidade = localRequest.Cidade,
            Estado = localRequest.Estado,
            Ativo = localRequest.Ativo,
            UsuarioId = localRequest.UsuarioId
        };
    }

    public static void UpdateLocalEntity(Local entity, LocalRequest request)
    {
        entity.DescricaoLocal = request.DescricaoLocal;
        entity.Nome = request.Nome;
        entity.Endereco = request.Endereco;
        entity.Bairro = request.Bairro;
        entity.Cidade = request.Cidade;
        entity.Estado = request.Estado;
        entity.Ativo = request.Ativo;
        entity.UsuarioId = request.UsuarioId;
    }
}