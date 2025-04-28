using OrganizadorEventos.Model;
using OrganizadorEventos.Request.Evento;

namespace OrganizadorEventos.Mappers;

public static class EventoMapper
{
    public static ListarEventosDTO ToRequest(this Evento entity)
    {
        return new ListarEventosDTO
        {
            Id = entity.Id,
            LocalId = entity.LocalId,
            Nome = entity.Nome,
            Descricao = entity.Descricao,
            DataInicio = entity.DataInicio,
            DataFinal = entity.DataFinal,
            Status = entity.Status,
            LocalDescricaoLocal = entity.LocalDescricaoLocal,
            LocalNome = entity.LocalNome,
            LocalEndereco = entity.LocalEndereco,
            LocalBairro = entity.LocalBairro,
            LocalCidade = entity.LocalCidade,
            LocalEstado = entity.LocalEstado
        };
    }
    
    public static Evento ToEntity(this CriarEventoDTO dto, Guid usuarioId)
    {
        return new Evento
        {
            Id = Guid.NewGuid(),
            UsuarioId = usuarioId,
            LocalId = dto.LocalId,
            Nome = dto.Nome,
            Descricao = dto.Descricao,
            DataInicio = dto.DataInicio,
            DataFinal = dto.DataFinal,
            Status = (int)dto.Status
        };
    }
}