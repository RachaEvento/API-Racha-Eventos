namespace OrganizadorEventos.DTOs.Evento;

public class RelatorioEventoDTO
{
    public string NomeEvento { get; set; } = null!;
    public string? Descricao { get; set; }
    public DateTime DataInicio { get; set; }
    public DateTime? DataFinal { get; set; }
    
    public string? Local { get; set; }
    public string Responsavel { get; set; } = null!;

    public int QuantidadeParticipantes { get; set; }
    public int QuantidadeConfirmados { get; set; }
    public int QuantidadeListasCusto { get; set; }

    public decimal CustoTotal { get; set; }
    public decimal MenorCusto { get; set; }
    public decimal MaiorCusto { get; set; }
}
