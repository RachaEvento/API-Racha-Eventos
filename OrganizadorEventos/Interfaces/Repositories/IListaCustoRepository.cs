using OrganizadorEventos.Model;

namespace OrganizadorEventos.Interfaces.Repositories;

public interface IListaCustoRepository
{
    Task<ListaCusto> CreateAsync(ListaCusto listaCusto);
    Task<List<ListaCusto>> GetAllByEventoIdAsync(Guid eventoId);
    Task<ListaCusto?> GetByIdAsync(Guid id);
    Task<List<ListaCusto>> ObterTodosComCustosAsync(Guid eventoId);

}