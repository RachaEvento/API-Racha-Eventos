namespace OrganizadorEventos.DTOs.Custos;

public class AdicionarParticipantesListaCustoDTO
{
    public Guid ListaCustoId { get; set; }
    public List<Guid> ParticipantesIds { get; set; } = new();
}