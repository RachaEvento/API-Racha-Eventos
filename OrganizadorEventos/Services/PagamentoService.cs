using OrganizadorEventos.DTOs.Participantes;
using OrganizadorEventos.Enum;
using OrganizadorEventos.Interfaces.Repositories;
using OrganizadorEventos.Interfaces.Services;
using OrganizadorEventos.Mappers;
using OrganizadorEventos.Model;

namespace OrganizadorEventos.Services;

public class PagamentoService : IPagamentoService
{
    private readonly IRelatorioEventoService _relatorioEventoService;
    private readonly IPixService _pixService;
    private readonly IPagamentoParticipanteRepository _pagamentoParticipanteRepository;    
    private readonly IParticipanteRepository _participanteRepository;


    public PagamentoService(IRelatorioEventoService relatorioEventoService, IPixService pixService, IPagamentoParticipanteRepository pagamentoParticipanteRepository, IParticipanteRepository participanteRepository)
    {
        _relatorioEventoService = relatorioEventoService;
        _pixService = pixService;
        _pagamentoParticipanteRepository = pagamentoParticipanteRepository;
        _participanteRepository = participanteRepository;
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
        
        return new ListaPagamentosDTO()
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
}