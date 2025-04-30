namespace OrganizadorEventos.DTOs.Custos;

public class CriarListaCustoDTO
{
    public Guid EventoId { get; set; }
    public string Nome { get; set; } = null!;
}