namespace OrganizadorEventos.DTOs.Custos;

public class AdicionarCustoDTO
{
    public Guid ListaCustoId { get; set; }
    public string Nome { get; set; } = null!;
    public decimal Valor { get; set; }
}