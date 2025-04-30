using OrganizadorEventos.DTOs.Custos;

namespace OrganizadorEventos.Interfaces.Services;

public interface IParticipanteListaCustoService
{
    Task AdicionarParticipantesAsync(AdicionarParticipantesListaCustoDTO dto);
    Task RemoverParticipantesAsync(RemoverParticipantesListaCustoDTO dto);
}