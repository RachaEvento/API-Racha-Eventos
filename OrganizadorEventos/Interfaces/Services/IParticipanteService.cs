using OrganizadorEventos.Request;

namespace OrganizadorEventos.Interfaces.Services;

public interface IParticipanteService
{
    Task AdicionarContatosComoParticipantesAsync(List<Guid> contatos, Guid eventoId);
    Task RemoverParticipantesAsync(List<Guid> participantes, Guid eventoId);
    Task<List<ContatoDTO>> ListarParticipantes(Guid eventoId);
    Task ConvidarTodosParticipantesEvento(Guid eventoId);
    Task ConvidarParticipante(Guid participanteId);
}