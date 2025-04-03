namespace OrganizadorEventos.Model;

public class EventCost
{
    public Guid Id { get; set; }
    public Guid EventId { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public Event Event { get; set; }
}