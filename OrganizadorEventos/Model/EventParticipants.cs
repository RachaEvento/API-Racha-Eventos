namespace OrganizadorEventos.Model;

public class EventParticipants
{
    public Guid EventId { get; set; }
    public bool IsPaying { get; set; }
    public Guid ContactId { get; set; }

    public bool IsHalfPrice { get; set; }
    public Event Event { get; set; }
    public Contact Contact { get; set; }
}