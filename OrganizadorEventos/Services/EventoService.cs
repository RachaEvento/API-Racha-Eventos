using System.Transactions;
using OrganizadorEventos.Enum;
using OrganizadorEventos.Interfaces.Repositories;
using OrganizadorEventos.Interfaces.Services;
using OrganizadorEventos.Mappers;
using OrganizadorEventos.Model;
using OrganizadorEventos.Request;
using OrganizadorEventos.Request.Evento;

namespace OrganizadorEventos.Services;

public class EventoService : IEventoService
{
    private readonly IContatoRepository _contatoRepository;
    private readonly IEventoRepository _eventoRepository;
    private readonly ILocalRepository _localRepository;
    private readonly IParticipanteRepository _participanteRepository;

    public EventoService(
        IEventoRepository eventoRepository, 
        IContatoRepository contatoRepository, 
        ILocalRepository localRepository, 
        IParticipanteRepository participanteRepository
    )
    {
        _eventoRepository = eventoRepository;
        _contatoRepository = contatoRepository;
        _localRepository = localRepository;
        _participanteRepository = participanteRepository;
    }

    public Task<List<Evento>> GetAllByUserAsync(Guid userId)
    {
        return _eventoRepository.GetAllByUserAsync(userId);
    }

    public async Task<Evento> CreateAsync(Evento entity, List<Guid>? contatosParticipantes)
    {
        var evento = await _eventoRepository.CreateAsync(entity);
        
        if (contatosParticipantes != null && contatosParticipantes.Any())
        {
            await _participanteRepository.CreateAllFromContactsAsync(contatosParticipantes, evento.Id);
        }

        return evento;
    }

    public async Task<Evento> GetByIdAsync(Guid eventoId)
    {
        return await _eventoRepository.GetByIdAsync(eventoId);
    }

    public async Task UpdateAsync(EditarEventoDTO dto, Guid eventoId)
    {
        var evento = await _eventoRepository.GetByIdAsync(eventoId);
        
        evento.Nome = dto.Nome;
        evento.DataFinal = dto.DataFinal;
        evento.DataInicio = dto.DataInicio;
        evento.Descricao = dto.Descricao;
        evento.LocalId = dto.LocalId;
        
        await _eventoRepository.UpdateAsync(evento);
    }
}