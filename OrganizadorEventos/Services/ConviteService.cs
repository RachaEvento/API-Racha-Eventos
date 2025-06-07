using System.Diagnostics;
using OrganizadorEventos.DTOs.Email;
using OrganizadorEventos.DTOs.Participantes;
using OrganizadorEventos.Enum;
using OrganizadorEventos.Interfaces.Repositories;
using OrganizadorEventos.Interfaces.Services;
using OrganizadorEventos.Mappers;

namespace OrganizadorEventos.Services;

public class ConviteService : IConviteService
{
    private readonly IEmailService _emailService;
    private readonly IEventoRepository _eventoRepository;
    private readonly IParticipanteRepository _participanteRepository;

    public ConviteService(IParticipanteRepository participanteRepository, IEmailService emailService,
        IEventoRepository eventoRepository)
    {
        _participanteRepository = participanteRepository;
        _emailService = emailService;
        _eventoRepository = eventoRepository;
    }

    public async Task ConvidarTodosParticipantesPendentesEvento(Guid eventoId)
    {
        var evento = await _eventoRepository.GetByIdAsync(eventoId);
        var usuario = evento.Usuario;
        var participantes = evento.Participantes.Where(p => p.Status == (int)StatusParticipante.Pendente)
            .Select(p => p.ToRequest()).ToList();

        _ = Task.Run(async () =>
        {
            foreach (var participante in participantes)
            {
                var convite = new ConviteEmailDTO
                {
                    ToEmail = participante.Email,
                    ConvidadoNome = participante.Nome,
                    QuemConvidaNome = usuario.UserName,
                    EventoNome = evento.Nome,
                    EventoData = evento.DataInicio,
                    CodigoConfirmacao = participante.Id.ToString()
                };

                try
                {
                    await _emailService.SendInvitationEmailAsync(convite);
                    Debug.WriteLine($"Enviado: {participante.Email}");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Falha ao enviar email para {participante.Email}, error: {ex.Message}");
                }
            }
        });
    }

    public async Task ConvidarParticipante(Guid participanteId)
    {
        var participante = await _participanteRepository.GetByIdAsync(participanteId);
        var evento = participante.Evento;
        var contato = participante.Contato;
        var usuario = contato.Usuario;

        var convite = new ConviteEmailDTO
        {
            ToEmail = contato.Email,
            ConvidadoNome = contato.Nome,
            QuemConvidaNome = usuario.UserName,
            EventoNome = evento.Nome,
            EventoData = evento.DataInicio,
            CodigoConfirmacao = participante.Id.ToString()
        };

        try
        {
            await _emailService.SendInvitationEmailAsync(convite);
            Debug.WriteLine($"Enviado: {contato.Email}");
            await ParticipantePendente(participante.Id);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Failed to send invitation to {contato.Email}, error: {ex.Message}");
            throw;
        }
    }

    public async Task ConfirmarParticipante(Guid participanteId)
    {
        var participante = await _participanteRepository.GetByIdAsync(participanteId);
        if (participante == null)
            throw new Exception("Participante não encontrado.");

        participante.Status = (int)StatusParticipante.Confirmado;
        await _participanteRepository.UpdateAsync(participante);
    }

    public async Task RecusarParticipante(Guid participanteId)
    {
        var participante = await _participanteRepository.GetByIdAsync(participanteId);
        if (participante == null)
            throw new Exception("Participante não encontrado.");

        participante.Status = (int)StatusParticipante.Recusado;
        await _participanteRepository.UpdateAsync(participante);
    }

    public async Task<ConviteParticipanteDTO> ConviteParticipante(Guid participanteId)
    {
        var participante = await _participanteRepository.GetByIdAsync(participanteId);

        return new ConviteParticipanteDTO
        {
            contatoParticipante = participante.ToRequest(),
            evento = participante.Evento.ToRequest()
        };
    }

    private async Task ParticipantePendente(Guid participanteId)
    {
        var participante = await _participanteRepository.GetByIdAsync(participanteId);
        if (participante == null)
            throw new Exception("Participante não encontrado.");

        participante.Status = (int)StatusParticipante.Pendente;
        await _participanteRepository.UpdateAsync(participante);
    }
}