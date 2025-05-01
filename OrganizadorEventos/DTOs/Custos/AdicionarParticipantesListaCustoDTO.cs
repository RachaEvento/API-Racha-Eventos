namespace OrganizadorEventos.DTOs.Custos;

public class AdicionarParticipantesListaCustoDTO
{
    public List<Guid> ParticipantesIds { get; set; } = new();
}