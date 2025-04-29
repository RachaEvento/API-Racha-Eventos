namespace OrganizadorEventos.DTOs.Custos;

public class CriarCustoDTO
{
    public Guid ListaCustoId { get; set; }
    public string Nome { get; set; } = null!;
    public decimal Valor { get; set; }
}