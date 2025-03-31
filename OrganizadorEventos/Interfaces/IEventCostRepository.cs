using OrganizadorEventos.Model;

namespace OrganizadorEventos.Interfaces;

public interface IEventCostRepository
{
    Task<EventCost> AddCostAsync(EventCost cost);
    Task<List<EventCost>> GetCostsByEventIdAsync(Guid eventId);
    Task<bool> RemoveCostAsync(Guid costId);
}