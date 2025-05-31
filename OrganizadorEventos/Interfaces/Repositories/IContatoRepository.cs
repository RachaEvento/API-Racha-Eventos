using OrganizadorEventos.Model;

namespace OrganizadorEventos.Interfaces.Repositories;

public interface IContatoRepository : ICrudRepository<Contato>
{
    Task<List<Contato>> GetAllByUserAsync(Guid usuarioId);
    Task<List<Contato>> GetAllByUserAndEventoAsync(Guid usuarioId, Guid eventoId);
}