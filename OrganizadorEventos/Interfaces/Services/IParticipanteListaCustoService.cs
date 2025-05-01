using OrganizadorEventos.DTOs.Custos;

namespace OrganizadorEventos.Interfaces.Services;

public interface IParticipanteListaCustoService
{
    Task AdicionarParticipantesAsync(Guid listaCustoId, AdicionarParticipantesListaCustoDTO dto);
    Task RemoverParticipantesAsync(Guid listaCustoId, RemoverParticipantesListaCustoDTO dto);
}