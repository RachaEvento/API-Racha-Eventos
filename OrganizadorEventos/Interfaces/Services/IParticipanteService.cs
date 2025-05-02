using OrganizadorEventos.DTOs.Participantes;
using OrganizadorEventos.Request;

namespace OrganizadorEventos.Interfaces.Services;

public interface IParticipanteService
{
    Task AdicionarContatosComoParticipantesAsync(AdicionarParticipanteDTO contatos, Guid eventoId);
    Task RemoverParticipantesAsync(RemoverParticipanteDTO participantes, Guid eventoId);
    Task<List<ContatoDTO>> ListarParticipantes(Guid eventoId);
    Task ConvidarTodosParticipantesEvento(Guid eventoId);
    Task ConvidarParticipante(Guid participanteId);
    Task ConfirmarParticipante(Guid participanteId);
    Task RecusarParticipante(Guid participanteId);
    Task<ConviteParticipanteDTO> ConviteParticipante(Guid participanteId);
}