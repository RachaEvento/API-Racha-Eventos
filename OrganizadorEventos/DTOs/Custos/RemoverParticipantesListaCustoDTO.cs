namespace OrganizadorEventos.DTOs.Custos;

public class RemoverParticipantesListaCustoDTO
{
    public Guid ListaCustoId { get; set; }
    public List<Guid> ParticipantesIds { get; set; } = new();
}