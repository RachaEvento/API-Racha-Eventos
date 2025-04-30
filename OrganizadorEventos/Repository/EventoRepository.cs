using Microsoft.EntityFrameworkCore;
using OrganizadorEventos.Data;
using OrganizadorEventos.Interfaces.Repositories;
using OrganizadorEventos.Model;

namespace OrganizadorEventos.Repository;

public class EventoRepository : IEventoRepository
{
    private readonly AppDbContext _context;
    protected readonly DbSet<Evento> _dbSet;

    public EventoRepository(AppDbContext context)
    {
        _context = context;
        _dbSet = context.Set<Evento>();
    }

    public Task<List<Evento>> GetAllByUserAsync(Guid usuarioId)
    {
        return _dbSet.Where(c => c.UsuarioId == usuarioId).ToListAsync();
    }

    public async Task<Evento> CreateAsync(Evento entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<Evento> GetByIdAsync(Guid eventoId)
    {
        return await _dbSet
            .Include(e => e.Usuario)
            .Include(e => e.Participantes)
            .ThenInclude(p => p.Contato)
            .FirstOrDefaultAsync(e => e.Id == eventoId);
    }

    public async Task UpdateAsync(Evento entity)
    {
        _dbSet.Update(entity);
        await _context.SaveChangesAsync();
    }
}