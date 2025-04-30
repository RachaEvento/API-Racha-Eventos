using OrganizadorEventos.Interfaces.Repositories;
using OrganizadorEventos.Interfaces.Services;
using OrganizadorEventos.Model;

namespace OrganizadorEventos.Services;

public class LocalService : CrudService<Local>, ILocalService
{
    private readonly ILocalRepository _localRepository;

    public LocalService(ILocalRepository localRepository) : base(localRepository)
    {
        _localRepository = localRepository;
    }

    public Task<List<Local>> GetAllByUserAsync(Guid userId)
    {
        return _localRepository.GetAllByUserAsync(userId);
    }
}