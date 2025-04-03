using Microsoft.EntityFrameworkCore;
using OrganizadorEventos.Data;
using OrganizadorEventos.Interfaces;
using OrganizadorEventos.Model;

public class EventCostRepository : IEventCostRepository
{
    private readonly AppDbContext _context;

    public EventCostRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<EventCost> AddCostAsync(EventCost cost)
    {
        _context.EventCosts.Add(cost);
        await _context.SaveChangesAsync();
        return cost;
    }

    public async Task<List<EventCost>> GetCostsByEventIdAsync(Guid eventId)
    {
        return await _context.EventCosts.Where(c => c.EventId == eventId).ToListAsync();
    }

    public async Task<bool> RemoveCostAsync(Guid costId)
    {
        var cost = await _context.EventCosts.FindAsync(costId);
        if (cost == null) return false;

        _context.EventCosts.Remove(cost);
        await _context.SaveChangesAsync();
        return true;
    }
}