using Microsoft.EntityFrameworkCore;
using OrganizadorEventos.Data;
using OrganizadorEventos.Interfaces.Repositories;
using OrganizadorEventos.Model;

namespace OrganizadorEventos.Repository;

public class ContatoRepository : CrudRepository<Contato>, IContatoRepository
{
    protected readonly DbSet<Contato> _dbSet;

    public ContatoRepository(AppDbContext context) : base(context)
    {
        _dbSet = context.Set<Contato>();
    }

    public Task<List<Contato>> GetAllByUserAsync(Guid usuarioId)
    {
        return _dbSet.Where(c => c.UsuarioId == usuarioId).ToListAsync();
    }
}