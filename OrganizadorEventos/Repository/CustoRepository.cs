using Microsoft.EntityFrameworkCore;
using OrganizadorEventos.Data;
using OrganizadorEventos.Interfaces.Repositories;
using OrganizadorEventos.Model;

namespace OrganizadorEventos.Repository;

public class CustoRepository : CrudRepository<Custo>, ICustoRepository
{
    private readonly AppDbContext _context;

    public CustoRepository(AppDbContext context): base(context)
    {
        _context = context;
    }

    public async Task<Custo> CreateAsync(Custo custo)
    {
        _context.Custos.Add(custo);
        await _context.SaveChangesAsync();
        return custo;
    }

    public async Task<List<Custo>> GetAllByListaCustoIdAsync(Guid listaCustoId)
    {
        return await _context.Custos
            .Where(c => c.ListaCustoId == listaCustoId)
            .ToListAsync();
    }
}