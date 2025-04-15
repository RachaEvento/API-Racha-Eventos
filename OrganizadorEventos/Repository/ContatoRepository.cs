using Microsoft.EntityFrameworkCore;
using OrganizadorEventos.Data;
using OrganizadorEventos.Interfaces;
using OrganizadorEventos.Interfaces.Repositories;
using OrganizadorEventos.Model;
using OrganizadorEventos.Request;

namespace OrganizadorEventos.Repository;

public class ContatoRepository : CrudRepository<Contato>, IContatoRepository
{
    private readonly AppDbContext _context;
    protected readonly DbSet<Contato> _dbSet;

    public ContatoRepository(AppDbContext context) : base(context)
    {
        _context = context;
        _dbSet = context.Set<Contato>();
    }

    public Task<List<Contato>> GetAllByUserAsync(Guid usuarioId)
    {
        return _dbSet.Where(c => c.UsuarioId == usuarioId).ToListAsync();
    }
}