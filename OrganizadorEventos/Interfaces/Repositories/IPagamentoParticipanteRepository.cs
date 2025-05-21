using OrganizadorEventos.Enum;
using OrganizadorEventos.Model;

namespace OrganizadorEventos.Interfaces.Repositories;

public interface IPagamentoParticipanteRepository : ICrudRepository<PagamentoParticipante>
{
    Task<List<PagamentoParticipante>> GetAllByParticipanteIdAsync(Guid participanteId);
}