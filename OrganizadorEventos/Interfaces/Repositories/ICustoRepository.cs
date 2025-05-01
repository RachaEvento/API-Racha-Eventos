using OrganizadorEventos.Model;

namespace OrganizadorEventos.Interfaces.Repositories;

public interface ICustoRepository : ICrudRepository<Custo>
{
    Task<Custo> CreateAsync(Custo custo);
    Task<List<Custo>> GetAllByListaCustoIdAsync(Guid listaCustoId);
}