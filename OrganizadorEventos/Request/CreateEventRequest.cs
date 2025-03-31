namespace OrganizadorEventos.Request;

public class CreateEventRequest
{
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime Date { get; set; }
    public decimal TotalPrice { get; set; }
    public int MaxParticipants { get; set; }
    public List<Guid> ContactIds { get; set; } = new();
    public List<Guid> IsPayingParticipants { get; set; } = new();
}