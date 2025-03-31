using OrganizadorEventos.Model;

namespace OrganizadorEventos.Interfaces;

public interface IEventRepository
{
    Task<Event> GetEventByIdAsync(Guid eventId);
    Task<Event> CreateEventAsync(Event evento);
    Task<Event> UpdateEventAsync(Event evento);
    Task<int> GetParticipantCountAsync(Guid eventId);
}