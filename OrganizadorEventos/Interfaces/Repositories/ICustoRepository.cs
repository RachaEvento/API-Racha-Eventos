using OrganizadorEventos.Model;

namespace OrganizadorEventos.Interfaces.Repositories;

public interface ICustoRepository
{
    Task<Custo> CreateAsync(Custo custo);
    Task<List<Custo>> GetAllByListaCustoIdAsync(Guid listaCustoId);
}