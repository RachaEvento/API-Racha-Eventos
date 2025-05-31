using OrganizadorEventos.Model;

namespace OrganizadorEventos.Interfaces.Services;

public interface IContatoService : ICrudService<Contato>
{
    Task<List<Contato>> GetAllByUserAsync(Guid userId);
    Task<List<Contato>> GetAllByUserAndEventoAsync(Guid userId, Guid eventoId);
}