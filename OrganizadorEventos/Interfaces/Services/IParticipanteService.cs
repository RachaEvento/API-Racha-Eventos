using OrganizadorEventos.DTOs.Participantes;
using OrganizadorEventos.Enum;
using OrganizadorEventos.Request;

namespace OrganizadorEventos.Interfaces.Services;

public interface IParticipanteService
{
    Task AdicionarContatosComoParticipantesAsync(AdicionarParticipanteDTO contatos, Guid eventoId);
    Task RemoverParticipantesAsync(RemoverParticipanteDTO participantes, Guid eventoId);
    Task<List<ParticipanteDTO>> ListarParticipantes(Guid eventoId);
}