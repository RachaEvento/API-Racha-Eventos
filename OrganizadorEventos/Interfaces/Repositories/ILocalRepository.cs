using OrganizadorEventos.Model;

namespace OrganizadorEventos.Interfaces.Repositories;

public interface ILocalRepository : ICrudRepository<Local>
{
    Task<List<Local>> GetAllByUserAsync(Guid usuarioId);
}