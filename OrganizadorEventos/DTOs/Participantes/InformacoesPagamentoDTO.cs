using OrganizadorEventos.Enum;
using OrganizadorEventos.Request;
using OrganizadorEventos.Request.Evento;

namespace OrganizadorEventos.DTOs.Participantes;

public class InformacoesPagamentoDTO
{
    public ContatoDTO contatoParticipante { get; set; }
    public ListarEventosDTO evento { get; set; }
    public StatusParticipante statusParticipante { get; set; }
    public StatusPagamento statusPagamento { get; set; }
    public string? stringPix { get; set; }
    public TipoChavePix? tipoChavePix { get; set; }
    public string? chavePix { get; set; }
    public decimal? valor { get; set; }
}