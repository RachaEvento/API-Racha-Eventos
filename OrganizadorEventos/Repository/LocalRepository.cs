using OrganizadorEventos.Data;
using OrganizadorEventos.Interfaces.Repositories;
using OrganizadorEventos.Model;

namespace OrganizadorEventos.Repository;

public class LocalRepository : CrudRepository<Local>, ILocalRepository
{
    private readonly AppDbContext _context;

    public LocalRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }
}