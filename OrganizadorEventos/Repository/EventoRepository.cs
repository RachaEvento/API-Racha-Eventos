using Microsoft.EntityFrameworkCore;
using OrganizadorEventos.Data;
using OrganizadorEventos.Interfaces;
using OrganizadorEventos.Interfaces.Repositories;
using OrganizadorEventos.Model;

namespace OrganizadorEventos.Repository;

public class EventoRepository : CrudRepository<Evento>, IEventoRepository
{
    private readonly AppDbContext _context;

    public EventoRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }
}