using OrganizadorEventos.Model;

namespace OrganizadorEventos.Interfaces.Services
{
    public interface ILocalService : ICrudService<Local>
    {
        Task<List<Local>> GetAllByUserAsync(Guid userId);
    }
}
