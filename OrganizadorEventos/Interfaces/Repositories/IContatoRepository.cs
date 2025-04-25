using OrganizadorEventos.Model;

namespace OrganizadorEventos.Interfaces.Repositories;

public interface IContatoRepository : ICrudRepository<Contato>
{
    Task<List<Contato>> GetAllByUserAsync(string usuarioId);
}