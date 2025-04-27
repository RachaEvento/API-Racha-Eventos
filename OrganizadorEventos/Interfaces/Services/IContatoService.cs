using OrganizadorEventos.Model;

namespace OrganizadorEventos.Interfaces.Services;

public interface IContatoService : ICrudService<Contato>
{
    Task<List<Contato>> GetAllByUserAsync(Guid userId);
}