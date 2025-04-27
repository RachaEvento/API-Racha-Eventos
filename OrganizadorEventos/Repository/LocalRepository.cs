using Microsoft.EntityFrameworkCore;
using OrganizadorEventos.Data;
using OrganizadorEventos.Interfaces.Repositories;
using OrganizadorEventos.Model;

namespace OrganizadorEventos.Repository;

public class LocalRepository : CrudRepository<Local>, ILocalRepository
{
    protected readonly DbSet<Local> _dbSet;

    public LocalRepository(AppDbContext context) : base(context)
    {
        _dbSet = context.Set<Local>();
    }

    public Task<List<Local>> GetAllByUserAsync(Guid usuarioId)
    {
        return _dbSet.Where(c => c.UsuarioId == usuarioId).ToListAsync();
    }
}