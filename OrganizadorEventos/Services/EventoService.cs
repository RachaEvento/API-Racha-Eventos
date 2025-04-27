using OrganizadorEventos.Interfaces.Repositories;
using OrganizadorEventos.Interfaces.Services;
using OrganizadorEventos.Model;

namespace OrganizadorEventos.Services;

public class EventoService : CrudService<Evento>, IEventoService
{
    private readonly IContatoRepository _contatoRepository;
    private readonly IEventoRepository _eventoRepository;
    private readonly ILocalRepository _localRepository;

    public EventoService(IEventoRepository eventoRepository, IContatoRepository contatoRepository, ILocalRepository localRepository) : base(eventoRepository)
    {
        _eventoRepository = eventoRepository;
        _contatoRepository = contatoRepository;
        _localRepository = localRepository;
    }
}