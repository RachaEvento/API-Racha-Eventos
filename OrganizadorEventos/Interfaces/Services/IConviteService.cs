using OrganizadorEventos.DTOs.Participantes;

namespace OrganizadorEventos.Interfaces.Services;

public interface IConviteService
{
    Task ConvidarTodosParticipantesEvento(Guid eventoId);
    Task ConvidarParticipante(Guid participanteId);
    Task ConfirmarParticipante(Guid participanteId);
    Task RecusarParticipante(Guid participanteId);
    Task<ConviteParticipanteDTO> ConviteParticipante(Guid participanteId);
}