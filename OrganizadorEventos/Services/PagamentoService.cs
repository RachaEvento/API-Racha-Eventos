using OrganizadorEventos.DTOs.Email;
using OrganizadorEventos.DTOs.Participantes;
using OrganizadorEventos.Enum;
using OrganizadorEventos.Interfaces.Repositories;
using OrganizadorEventos.Interfaces.Services;
using OrganizadorEventos.Mappers;
using OrganizadorEventos.Model;

namespace OrganizadorEventos.Services;

public class PagamentoService : IPagamentoService
{
    private readonly IPagamentoParticipanteRepository _pagamentoParticipanteRepository;
    private readonly IParticipanteRepository _participanteRepository;
    private readonly IEmailService _emailService;
    private readonly IEventoRepository _eventoRepository;


    public PagamentoService(IRelatorioEventoService relatorioEventoService, IPixService pixService, IPagamentoParticipanteRepository pagamentoParticipanteRepository, IParticipanteRepository participanteRepository, IEmailService emailService, IEventoRepository eventoRepository)
    {
        _relatorioEventoService = relatorioEventoService;
        _pixService = pixService;
        _pagamentoParticipanteRepository = pagamentoParticipanteRepository;
        _participanteRepository = participanteRepository;
        _emailService = emailService;
        _eventoRepository = eventoRepository;
    }

    public async Task<InformacoesPagamentoDTO> InformacoesPagamento(Guid participanteId)
    {
        var participante = await _participanteRepository.GetByIdAsync(participanteId);
        var custoParticipante = await _relatorioEventoService.CalcularCustoParticipanteAsync(participante);

        var usuario = participante.Evento.Usuario;

        var nomeLimpo = participante.Contato.Nome.Replace(" ", "");
        var nomeSeguro = nomeLimpo.Substring(0, Math.Min(15, nomeLimpo.Length)).ToUpper();
        var pix = await _pixService.GeneratePixQrCodeAsync(usuario.ChavePix, usuario.UserName, "Criciúma",
            custoParticipante.Custo, participante.Evento.Nome, $"PGT{nomeSeguro}");

        var pagamentos = participante.Pagamento?.OrderByDescending(pag => pag.DataPagamento);
        var statusPagamento = (StatusPagamento?)pagamentos?.FirstOrDefault()?.Status ?? StatusPagamento.Pendente;

        return new InformacoesPagamentoDTO
        {
            contatoParticipante = custoParticipante.Participante,
            evento = participante.Evento.ToRequest(),
            statusParticipante = (StatusParticipante)participante.Status,
            statusPagamento = statusPagamento,
            stringPix = pix,
            tipoChavePix = usuario.TipoChavePix,
            chavePix = usuario.ChavePix,
            valor = custoParticipante.Custo
        };
    }

    public async Task SalvarPagamento(Guid participanteId, string fileId)
    {
        var pagamento = new PagamentoParticipante
        {
            Id = Guid.NewGuid(),
            ParticipanteId = participanteId,
            Comprovante = fileId,
            Status = (int)StatusPagamento.Enviado,
            DataPagamento = DateTime.UtcNow
        };

        await _pagamentoParticipanteRepository.CreateAsync(pagamento);
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
            statusPagamento = (StatusPagamento)p.Status,
            comprovante = p.Comprovante
        }).ToList();

        return new ListaPagamentosDTO
        {
            participante = participante.ToRequest(),
            statusParticipante = (StatusParticipante)participante.Status,
            valor = custoParticipante?.Custo,
            pagamentos = listaPagamentos
        };
    }

    public async Task AlterarPagamentoParticipante(Guid pagamentoId, StatusPagamento status)
    {
        var pagamentoParticipante = await _pagamentoParticipanteRepository.GetByIdAsync(pagamentoId);
        pagamentoParticipante.Status = (int)status;
        await _pagamentoParticipanteRepository.UpdateAsync(pagamentoParticipante);
    }

    public async Task CobrarTodosPagamentosPendentesEventos(Guid eventoId)
    {
        var evento = await _eventoRepository.GetByIdAsync(eventoId);
        var usuario = evento.Usuario;
        var participantes = evento.Participantes.Where(p => p.Status == (int)StatusParticipante.Pendente).Select(p => p.ToRequest()).ToList();
        
        _ = Task.Run(async () =>
        {
            foreach (var participante in participantes)
            {
                var custoParticipante = await _relatorioEventoService.CalcularCustoParticipanteAsync(participante);
                
                var cobranca = new CobrancaEmailDTO()
                {
                    ToEmail = participante.Email,
                    ConvidadoNome = participante.Nome,
                    QuemConvidaNome = usuario.UserName,
                    EventoNome = evento.Nome,
                    Valor = custoParticipante.Custo,
                    CodigoParticipante = participante.Id.ToString()
                };
                
                try
                {
                    await _emailService.SendChargeEmailAsync(cobranca);
                    System.Diagnostics.Debug.WriteLine($"Enviado: {participante.Email}");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Falha ao enviar email de cobrança para {participante.Email}, error: {ex.Message}");
                }
            }
        });
    }

    public async Task CobrarParticipanteEvento(Guid participanteId)
    {
        var participante = await _participanteRepository.GetByIdAsync(participanteId);
        var evento = participante.Evento;
        var contato = participante.Contato;
        var usuario = contato.Usuario;
        
        var custoParticipante = await _relatorioEventoService.CalcularCustoParticipanteAsync(participante);
        
        var cobranca = new CobrancaEmailDTO()
        {
            ToEmail = contato.Email,
            ConvidadoNome = contato.Nome,
            QuemConvidaNome = usuario.UserName,
            EventoNome = evento.Nome,
            Valor = custoParticipante.Custo,
            CodigoParticipante = participante.Id.ToString()
        };

        try
        {
            await _emailService.SendChargeEmailAsync(cobranca);
            System.Diagnostics.Debug.WriteLine($"Enviado cobrança: {contato.Email}");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Failed to send charge to {contato.Email}, error: {ex.Message}");
            throw;
        }
    }
}