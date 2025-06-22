using OrganizadorEventos.DTOs.Evento;

namespace OrganizadorEventos.Model;

public class ResultadoRelatorioEvento
{
    public bool Sucesso { get; set; }
    public string? MensagemErro { get; set; }
    public RelatorioEventoDTO? Relatorio { get; set; }
}
