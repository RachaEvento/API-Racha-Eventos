namespace OrganizadorEventos.Request;

public class AddParticipantsRequest
{
    public List<Guid> ContactIds { get; set; } = new();
    public List<Guid> IsPayingParticipants { get; set; } = new();
    public List<Guid> IsHalfPriceParticipants { get; set; } = new();
}