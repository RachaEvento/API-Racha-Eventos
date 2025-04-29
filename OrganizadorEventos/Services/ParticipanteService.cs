using OrganizadorEventos.Interfaces.Repositories;
using OrganizadorEventos.Interfaces.Services;
using OrganizadorEventos.Mappers;
using OrganizadorEventos.Request;

namespace OrganizadorEventos.Services;

public class ParticipanteService : IParticipanteService
{
    private readonly IParticipanteRepository _participanteRepository;

    public ParticipanteService(IParticipanteRepository participanteRepository)
    {
        _participanteRepository = participanteRepository;
    }

    public async Task AdicionarParticipantesAsync(List<Guid> contatoParticipantes, Guid eventoId)
    {
        await _participanteRepository.CreateAllFromContactsAsync(contatoParticipantes, eventoId);
    }

    public async Task RemoverParticipantesAsync(List<Guid> contatoParticipantes, Guid eventoId)
    {
        await _participanteRepository.RemoveParticipantsAsync(contatoParticipantes, eventoId);
    }

    public async Task<List<ContatoDTO>> ListarParticipantes(Guid eventoId)
    {
        var participantes = await _participanteRepository.GetAllByEventId(eventoId);
        var contatosParticipantes = participantes.Select(p => p.Contato.ToRequest()).ToList();
        return contatosParticipantes;
    }
}