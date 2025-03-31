using Microsoft.EntityFrameworkCore;
using OrganizadorEventos.Data;
using OrganizadorEventos.Interfaces;
using OrganizadorEventos.Model;

namespace OrganizadorEventos.Repository;

public class EventRepository : IEventRepository
{
    private readonly AppDbContext _dbContext;

    public EventRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Event> CreateEventAsync(Event evento)
    {
        if (evento.MaxParticipants <= 0)
            throw new ArgumentException("O evento precisa ter pelo menos 1 participante.");

        _dbContext.Events.Add(evento);
        await _dbContext.SaveChangesAsync();
        return evento;
    }

    public async Task<Event> GetEventByIdAsync(Guid eventId)
    {
        return await _dbContext.Events
            .FirstOrDefaultAsync(e => e.Id == eventId);
    }

    public async Task<Event> UpdateEventAsync(Event evento)
    {
        _dbContext.Events.Update(evento);
        await _dbContext.SaveChangesAsync();
        return evento;
    }

    public async Task<int> GetParticipantCountAsync(Guid eventId)
    {
        return await _dbContext.EventParticipants
            .Where(ep => ep.EventId == eventId)
            .CountAsync();
    }
}