using OrganizadorEventos.Model;

namespace OrganizadorEventos.Interfaces.Repositories;

public interface IEventoRepository
{
    Task<List<Evento>> GetAllByUserAsync(Guid usuarioId);
    Task<Evento> CreateAsync(Evento entity);
    Task<Evento> GetByIdAsync(Guid eventoId);
    Task UpdateAsync(Evento entity);
}