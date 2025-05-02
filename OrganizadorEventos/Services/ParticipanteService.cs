using OrganizadorEventos.DTOs.Email;
using OrganizadorEventos.DTOs.Participantes;
using OrganizadorEventos.Enum;
using OrganizadorEventos.Interfaces.Repositories;
using OrganizadorEventos.Interfaces.Services;
using OrganizadorEventos.Mappers;
using OrganizadorEventos.Request;
using OrganizadorEventos.Request.Evento;

namespace OrganizadorEventos.Services;

public class ParticipanteService : IParticipanteService
{
    private readonly IParticipanteRepository _participanteRepository;
    private readonly IEmailService _emailService;
    private readonly IContatoRepository _contatoRepository;
    private readonly IEventoRepository _eventoRepository;

    public ParticipanteService(IParticipanteRepository participanteRepository, IEmailService emailService, IContatoRepository contatoRepository, IEventoRepository eventoRepository)
    {
        _participanteRepository = participanteRepository;
        _emailService = emailService;
        _contatoRepository = contatoRepository;
        _eventoRepository = eventoRepository;
    }

    public async Task AdicionarContatosComoParticipantesAsync(AdicionarParticipanteDTO contatos, Guid eventoId)
    {
        await _participanteRepository.CreateAllFromContactsAsync(contatos.ContatoIds, eventoId);
    }

    public async Task RemoverParticipantesAsync(RemoverParticipanteDTO participantes, Guid eventoId)
    {
        await _participanteRepository.RemoveParticipantsAsync(participantes.ParticipantesIds, eventoId);
    }

    public async Task<List<ContatoDTO>> ListarParticipantes(Guid eventoId)
    {
        var participantes = await _participanteRepository.GetAllByEventId(eventoId);
        var contatosParticipantes = participantes.Select(p =>
        {
            var dto = p.Contato.ToRequest();
            dto.Id = p.Id;
            return dto;
        }).ToList();
        
        return contatosParticipantes;
    }
    
    public async Task ConvidarParticipante(Guid participanteId)
    {
        var participante = await _participanteRepository.GetByIdAsync(participanteId);
        var evento = participante.Evento;
        var contato = participante.Contato;
        var usuario = contato.Usuario;
        
        var convite = new ConviteEmailDTO()
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
            System.Diagnostics.Debug.WriteLine($"Enviado: {contato.Email}");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Failed to send invitation to {contato.Email}, error: {ex.Message}");
        }
    }

    public async Task ConfirmarParticipante(Guid participanteId)
    {
        var participante = await _participanteRepository.GetByIdAsync(participanteId);
        if (participante == null)
            throw new Exception("Participante não encontrado.");
        
        participante.Status = (int)StatusParticipante.Confirmado;

        if (participante.Status == (int)StatusParticipante.Pendente)
        {
            await _participanteRepository.UpdateAsync(participante);
        }
    }

    public async Task RecusarParticipante(Guid participanteId)
    {
        var participante = await _participanteRepository.GetByIdAsync(participanteId);
        if (participante == null)
            throw new Exception("Participante não encontrado.");
        
        participante.Status = (int)StatusParticipante.Recusado;
        
        if (participante.Status == (int)StatusParticipante.Pendente)
        {
            await _participanteRepository.UpdateAsync(participante);
        }
    }

    public async Task<ConviteParticipanteDTO> ConviteParticipante(Guid participanteId)
    {
        var participante = await _participanteRepository.GetByIdAsync(participanteId);
        
        var contatoParticipante = new ContatoDTO()
        {
            Id = participante.Id,
            Nome = participante.Contato.Nome,
            Email = participante.Contato.Email,
            Telefone = participante.Contato.Telefone,
            Ativo = participante.Contato.Ativo
        };
        var evento = participante.Evento.ToRequest();
        
        return new ConviteParticipanteDTO()
        {
            contatoParticipante = contatoParticipante,
            evento = evento,
            status = (StatusParticipante)participante.Status
        };
    }

    public async Task ConvidarTodosParticipantesEvento(Guid eventoId)
    {
        var evento = await _eventoRepository.GetByIdAsync(eventoId);
        var usuario = evento.Usuario;
        var participantes = evento.Participantes.Select(p => p.Contato).ToList();
        
        _ = Task.Run(async () =>
        {
            foreach (var participante in participantes)
            {
                var convite = new ConviteEmailDTO()
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
                    System.Diagnostics.Debug.WriteLine($"Enviado: {participante.Email}");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Failed to send invitation to {participante.Email}, error: {ex.Message}");
                }
            }
        });
    }
    
    
}