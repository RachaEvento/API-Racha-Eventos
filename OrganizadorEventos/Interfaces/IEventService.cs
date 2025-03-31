using OrganizadorEventos.Model;
using OrganizadorEventos.Request;

namespace OrganizadorEventos.Interfaces;

public interface IEventService
{
    Task<Event> CreateEventAsync(CreateEventRequest request);
    Task<Event> FinalizeEventAsync(Guid eventId);
    Task<EventCost> AddCostToEventAsync(Guid eventId, string description, decimal amount);

    Task<Event> AddContactsToEventAsync(Guid eventId, List<Guid> contactIds, List<Guid> isPayingParticipants,
        List<Guid> isHalfPriceParticipants);

    Task<List<EventCost>> GetEventCostsAsync(Guid eventId);
}