using OrganizadorEventos.Interfaces.Repositories;
using OrganizadorEventos.Interfaces.Services;
using OrganizadorEventos.Model;

namespace OrganizadorEventos.Services;

public class ContatoService : CrudService<Contato>, IContatoService
{
    private readonly IContatoRepository _contatoRepository;

    public ContatoService(IContatoRepository contatoRepository) : base (contatoRepository)
    {
        _contatoRepository = contatoRepository;
    }

    public Task<List<Contato>> GetAllByUserAsync(Guid userId)
    {
        return _contatoRepository.GetAllByUserAsync(userId);
    }
}