using OrganizadorEventos.Enum;
using OrganizadorEventos.Interfaces;
using OrganizadorEventos.Model;
using OrganizadorEventos.Request;

namespace OrganizadorEventos.Services;

public class EventService : IEventService
{
    private readonly IContactRepository _contactRepository;
    private readonly IEventCostRepository _eventCostRepository;
    private readonly IEventRepository _eventRepository;
    private readonly ILocationRepository _locationRepository;

    public EventService(IEventRepository eventRepository, IContactRepository contactRepository,
        IEventCostRepository eventCostRepository, ILocationRepository locationRepository)
    {
        _eventRepository = eventRepository;
        _contactRepository = contactRepository;
        _eventCostRepository = eventCostRepository;
        _locationRepository = locationRepository;
    }

    public async Task<Event> CreateEventAsync(CreateEventRequest request, string userId)
    {
        var evento = new Event
        {
            Name = request.Name,
            Description = request.Description,
            Date = request.Date,
            TotalPrice = request.TotalPrice,
            MaxParticipants = request.MaxParticipants,
            CreatedAt = DateTime.UtcNow,
            Status = EventStatus.Aberto,
            UserId = userId,
            EventParticipants = new List<EventParticipants>()
        };
        if (request.LocationId.HasValue)
        {
            var location = await _locationRepository.GetLocationByIdAsync(request.LocationId.Value);
            if (location != null)
            {
                evento.LocationId = location.Id;
                evento.Location = location;
            }
            else
            {
                throw new ArgumentException($"Localização com ID {request.LocationId} não encontrada.");
            }
        }

        foreach (var contactId in request.ContactIds)
        {
            var contact = await _contactRepository.GetContactByIdAsync(contactId, userId);
            if (contact == null)
                throw new ArgumentException($"Contato com ID {contactId} não encontrado.");

            var isPaying = request.IsPayingParticipants.Contains(contactId);

            evento.EventParticipants.Add(new EventParticipants
            {
                EventId = evento.Id,
                ContactId = contactId,
                IsPaying = isPaying
            });
        }

        var createdEvent = await _eventRepository.CreateEventAsync(evento);

        return createdEvent;
    }

    public async Task<Event> AddContactsToEventAsync(Guid eventId, List<Guid> contactIds,
        List<Guid> isPayingParticipants, List<Guid> isHalfPriceParticipants, string userId)
    {
        var evento = await _eventRepository.GetEventByIdAsync(eventId);
        if (evento == null)
            throw new ArgumentException($"Evento com ID {eventId} não encontrado.");

        foreach (var contactId in contactIds)
        {
            var contact = await _contactRepository.GetContactByIdAsync(contactId, userId);
            if (contact == null)
                throw new ArgumentException($"Contato com ID {contactId} não encontrado.");

            var isPaying = isPayingParticipants.Contains(contactId);

            var isHalfPrice = isHalfPriceParticipants.Contains(contactId);

            var existingParticipant = evento.EventParticipants.FirstOrDefault(ep => ep.ContactId == contactId);
            if (existingParticipant != null)
                throw new InvalidOperationException($"O participante com ID {contactId} já foi adicionado ao evento.");

            evento.EventParticipants.Add(new EventParticipants
            {
                EventId = evento.Id,
                ContactId = contactId,
                IsPaying = isPaying,
                IsHalfPrice = isHalfPrice
            });
        }

        await _eventRepository.UpdateEventAsync(evento);

        return evento;
    }


    public async Task<EventCost> AddCostToEventAsync(Guid eventId, string description, decimal amount)
    {
        var evento = await _eventRepository.GetEventByIdAsync(eventId);
        if (evento == null)
            throw new ArgumentException($"Evento com ID {eventId} não encontrado.");

        var newCost = new EventCost
        {
            EventId = eventId,
            Description = description,
            Amount = amount
        };

        await _eventCostRepository.AddCostAsync(newCost);
        evento.TotalPrice += amount;
        await _eventRepository.UpdateEventAsync(evento);

        return newCost;
    }


    public async Task<List<EventCost>> GetEventCostsAsync(Guid eventId)
    {
        return await _eventCostRepository.GetCostsByEventIdAsync(eventId);
    }
}