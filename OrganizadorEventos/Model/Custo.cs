using Microsoft.EntityFrameworkCore;

namespace OrganizadorEventos.Model;

public class Custo
{
    public Guid Id { get; set; }
    public Guid ListaCustoId { get; set; }
    public string Nome { get; set; } = null!;
    public decimal Valor { get; set; }

    public ListaCusto ListaCusto { get; set; } = null!;
}