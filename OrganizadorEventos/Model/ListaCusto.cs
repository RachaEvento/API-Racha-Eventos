namespace OrganizadorEventos.Model;

public class ListaCusto
{
    public Guid Id { get; set; }
    public Guid EventoId { get; set; }
    public string Nome { get; set; } = null!;

    public Evento Evento { get; set; } = null!;
    public ICollection<Custo> Custos { get; set; } = new List<Custo>();

    public ICollection<ParticipanteListaCusto> ParticipanteListaCustos { get; set; } = new List<ParticipanteListaCusto>();
}