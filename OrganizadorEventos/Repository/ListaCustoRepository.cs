using Microsoft.EntityFrameworkCore;
using OrganizadorEventos.Data;
using OrganizadorEventos.Interfaces.Repositories;
using OrganizadorEventos.Model;

namespace OrganizadorEventos.Repository;

public class ListaCustoRepository : IListaCustoRepository
{
    private readonly AppDbContext _context;

    public ListaCustoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ListaCusto> CreateAsync(ListaCusto listaCusto)
    {
        _context.ListaCustos.Add(listaCusto);
        await _context.SaveChangesAsync();
        return listaCusto;
    }

    public async Task<List<ListaCusto>> GetAllByEventoIdAsync(Guid eventoId)
    {
        return await _context.ListaCustos
            .Where(l => l.EventoId == eventoId)
            .Include(l => l.Custos)
            .ToListAsync();
    }

    public async Task<ListaCusto?> GetByIdAsync(Guid id)
    {
        return await _context.ListaCustos
            .Include(l => l.Custos)
            .FirstOrDefaultAsync(l => l.Id == id);
    }
    public async Task<List<ListaCusto>> ObterTodosComCustosAsync(Guid eventoId)
    {
        return await _context.ListaCustos
            .Where(l => l.EventoId == eventoId) 
            .Include(l => l.Custos)
            .ToListAsync();
    }

}