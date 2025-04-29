using OrganizadorEventos.Request;

namespace OrganizadorEventos.Interfaces.Services;

public interface IParticipanteService
{
    Task AdicionarParticipantesAsync(List<Guid> contatoParticipantes, Guid eventoId);
    Task RemoverParticipantesAsync(List<Guid> contatoParticipantes, Guid eventoId);
    Task<List<ContatoDTO>> ListarParticipantes(Guid eventoId);
}