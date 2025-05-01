namespace OrganizadorEventos.DTOs.Custos;

public class RemoverParticipantesListaCustoDTO
{
    public List<Guid> ParticipantesIds { get; set; } = new();
}