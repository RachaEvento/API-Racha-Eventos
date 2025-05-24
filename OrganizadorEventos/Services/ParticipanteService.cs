using Google.Apis.Drive.v3.Data;
using OrganizadorEventos.DTOs.Email;
using OrganizadorEventos.DTOs.Participantes;
using OrganizadorEventos.Enum;
using OrganizadorEventos.Interfaces.Repositories;
using OrganizadorEventos.Interfaces.Services;
using OrganizadorEventos.Mappers;
using OrganizadorEventos.Model;
using OrganizadorEventos.Repository;
using OrganizadorEventos.Request;
using OrganizadorEventos.Request.Evento;

namespace OrganizadorEventos.Services;

public class ParticipanteService : IParticipanteService
{
    private readonly IParticipanteRepository _participanteRepository;

    public ParticipanteService(IParticipanteRepository participanteRepository)
    {
        _participanteRepository = participanteRepository;
    }

    public async Task AdicionarContatosComoParticipantesAsync(AdicionarParticipanteDTO contatos, Guid eventoId)
    {
        await _participanteRepository.CreateAllFromContactsAsync(contatos.ContatoIds, eventoId);
    }

    public async Task RemoverParticipantesAsync(RemoverParticipanteDTO participantes, Guid eventoId)
    {
        await _participanteRepository.RemoveParticipantsAsync(participantes.ParticipantesIds, eventoId);
    }

    public async Task<List<ParticipanteDTO>> ListarParticipantes(Guid eventoId)
    {
        var participantes = await _participanteRepository.GetAllByEventId(eventoId);
        var contatosParticipantes = participantes.Select(p => p.ToRequest()).ToList();
        
        return contatosParticipantes;
    }
}