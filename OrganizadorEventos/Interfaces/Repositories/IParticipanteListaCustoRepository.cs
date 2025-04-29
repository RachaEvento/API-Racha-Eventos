namespace OrganizadorEventos.Interfaces.Repositories;

public interface IParticipanteListaCustoRepository
{
    Task CreateAllAsync(Guid listaCustoId, List<Guid> participanteIds);
    Task RemoveAllAsync(Guid listaCustoId, List<Guid> participanteIds);
}