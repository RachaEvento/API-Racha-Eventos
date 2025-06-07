using OrganizadorEventos.Enum;

namespace OrganizadorEventos.DTOs.Participantes;

public class ListaPagamentosDTO
{
    public ParticipanteDTO participante { get; set; }
    public decimal? valor { get; set; }
    public StatusParticipante statusParticipante { get; set; }
    public List<PagamentosDTO> pagamentos { get; set; } = new();
}

public class PagamentosDTO
{
    public Guid pagamentoId { get; set; }
    public DateTime dataPagamento { get; set; }

    public StatusPagamento statusPagamento { get; set; }
    public string comprovante { get; set; }
}