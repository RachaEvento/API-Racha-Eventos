using Microsoft.EntityFrameworkCore;
using OrganizadorEventos.Data;
using OrganizadorEventos.Interfaces.Repositories;
using OrganizadorEventos.Model;

namespace OrganizadorEventos.Repository;

public class PagamentoParticipanteRepository : CrudRepository<PagamentoParticipante>, IPagamentoParticipanteRepository
{
    private readonly AppDbContext _context;
    protected readonly DbSet<PagamentoParticipante> _dbSet;

    public PagamentoParticipanteRepository(AppDbContext context) : base(context)
    {
        _context = context;
        _dbSet = context.Set<PagamentoParticipante>();
    }

    public async Task<List<PagamentoParticipante>> GetAllByParticipanteIdAsync(Guid participanteId)
    {
        return await _dbSet.Where(pp => pp.ParticipanteId == participanteId).ToListAsync();
    }
}