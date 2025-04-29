using OrganizadorEventos.Model;
using OrganizadorEventos.Request.Evento;

namespace OrganizadorEventos.Interfaces.Services;

public interface IEventoService
{
    Task<List<Evento>> GetAllByUserAsync(Guid userId);
    Task<Evento> CreateAsync(Evento entity, List<Guid>? contatosParticipantes);
    Task<Evento> GetByIdAsync(Guid eventoId);
    Task UpdateAsync(EditarEventoDTO dto, Guid eventoId);
}