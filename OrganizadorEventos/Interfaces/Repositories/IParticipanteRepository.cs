using OrganizadorEventos.Model;

namespace OrganizadorEventos.Interfaces.Repositories;

public interface IParticipanteRepository : ICrudRepository<Participante>
{
    Task<List<Participante>> CreateAllFromContactsAsync(List<Guid> contatosParticipantes, Guid eventoId);
    Task RemoveParticipantsAsync(List<Guid> contatosParticipantes, Guid eventoId);
    Task<List<Participante>> GetAllByEventId(Guid eventoId);
    new Task<Participante> GetByIdAsync(Guid id);
    Task<List<Participante>> GetAllConfirmedByEventIdAsync(Guid eventoId);
}