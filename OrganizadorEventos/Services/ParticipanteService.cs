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
    private readonly IEmailService _emailService;
    private readonly IContatoRepository _contatoRepository;
    private readonly IEventoRepository _eventoRepository;
    private readonly IRelatorioEventoService _relatorioEventoService;
    private readonly IPixService _pixService;
    private readonly IPagamentoParticipanteRepository _pagamentoParticipanteRepository;

    public ParticipanteService(IParticipanteRepository participanteRepository, IEmailService emailService, IContatoRepository contatoRepository, IEventoRepository eventoRepository, IPixService pixService, IRelatorioEventoService relatorioEventoService, IPagamentoParticipanteRepository pagamentoParticipanteRepository)
    {
        _participanteRepository = participanteRepository;
        _emailService = emailService;
        _contatoRepository = contatoRepository;
        _eventoRepository = eventoRepository;
        _pixService = pixService;
        _relatorioEventoService = relatorioEventoService;
        _pagamentoParticipanteRepository = pagamentoParticipanteRepository;
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

        if (participante.Status == (int)StatusParticipante.Pendente)
        {
            participante.Status = (int)StatusParticipante.Confirmado;
            await _participanteRepository.UpdateAsync(participante);
        }
    }

    public async Task RecusarParticipante(Guid participanteId)
    {
        var participante = await _participanteRepository.GetByIdAsync(participanteId);
        if (participante == null)
            throw new Exception("Participante não encontrado.");
        
        if (participante.Status == (int)StatusParticipante.Pendente)
        {
            participante.Status = (int)StatusParticipante.Recusado;
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

    public async Task<InformacoesPagamentoDTO> InformacoesPagamento(Guid participanteId)
    {
        var participante = await _participanteRepository.GetByIdAsync(participanteId);
        var custoParticipante = await _relatorioEventoService.CalcularCustoParticipanteAsync(participante);
        
        var usuario = participante.Evento.Usuario;
        
        var nomeLimpo = participante.Contato.Nome.Replace(" ", "");
        var nomeSeguro = nomeLimpo.Substring(0, Math.Min(15, nomeLimpo.Length)).ToUpper();
        var pix = await _pixService.GeneratePixQrCodeAsync(usuario.ChavePix, usuario.UserName, "Criciúma", custoParticipante.Custo, participante.Evento.Nome, $"PGT{nomeSeguro}");

        var pagamentos = participante.Pagamento?.OrderByDescending(pag => pag.DataPagamento);
        var statusPagamento = (StatusPagamento?)pagamentos?.FirstOrDefault()?.Status ?? StatusPagamento.Pendente;

        return new InformacoesPagamentoDTO()
        {
            contatoParticipante = custoParticipante.Participante,
            evento = participante.Evento.ToRequest(),
            statusParticipante = (StatusParticipante)participante.Status,
            statusPagamento = statusPagamento,
            stringPix = pix,
            tipoChavePix = usuario.TipoChavePix,
            chavePix = usuario.ChavePix,
            valor = custoParticipante.Custo,
        };
    }

    public async Task SalvarPagamento(Guid participanteId, string fileId)
    {
        var pagamento = new PagamentoParticipante()
        {
            Id = Guid.NewGuid(),
            ParticipanteId = participanteId,
            Comprovante = fileId,
            Status = (int)StatusPagamento.Enviado,
            DataPagamento = DateTime.UtcNow
        };

        await _pagamentoParticipanteRepository.CreateAsync(pagamento);
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

    public async Task<ListaPagamentosDTO> ListarPagamentoParticipante(Guid participanteId)
    {
        var participante = await _participanteRepository.GetByIdAsync(participanteId);
        var custoParticipante = await _relatorioEventoService.CalcularCustoParticipanteAsync(participante);
        var pagamentos = await _pagamentoParticipanteRepository.GetAllByParticipanteIdAsync(participanteId);
        
        var listaPagamentos = pagamentos.Select(p => new PagamentosDTO
        {
            dataPagamento = p.DataPagamento,
            pagamentoId = p.Id,
            statusPagamento = (StatusPagamento)p.Status
        }).ToList();
        
        return new ListaPagamentosDTO()
        {
            participante = participante.ToRequest(),
            statusParticipante = (StatusParticipante)participante.Status,
            valor = custoParticipante.Custo,
            pagamentos = listaPagamentos
        };
    }

    public async Task AlterarPagamentoParticipante(Guid pagamentoId, StatusPagamento status)
    {
        var pagamentoParticipante = await _pagamentoParticipanteRepository.GetByIdAsync(pagamentoId);
        pagamentoParticipante.Status = (int)status;
        await _pagamentoParticipanteRepository.UpdateAsync(pagamentoParticipante);
    }
}