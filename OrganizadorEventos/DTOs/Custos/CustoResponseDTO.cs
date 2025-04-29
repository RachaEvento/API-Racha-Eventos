namespace OrganizadorEventos.DTOs.Custos;

public class CustoResponseDTO
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = null!;
    public decimal Valor { get; set; }
}