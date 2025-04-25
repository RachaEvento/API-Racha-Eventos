namespace OrganizadorEventos.Model;

public class Evento
{
    public Guid Id { get; set; }
    public string UsuarioId { get; set; }
    public Guid? LocalId { get; set; }
    public string Nome { get; set; } = null!;
    public string? Descricao { get; set; }
    public DateTime DataInicio { get; set; }
    public DateTime? DataFinal { get; set; }
    public int Status { get; set; }

    public ApplicationUser Usuario { get; set; } = null!;
    public Local? Local { get; set; }
    public ICollection<ListaCusto> ListaCustos { get; set; } = new List<ListaCusto>();
    public ICollection<Participante> Participantes { get; set; } = new List<Participante>();
}