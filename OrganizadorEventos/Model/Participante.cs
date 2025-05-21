namespace OrganizadorEventos.Model;

public class Participante
{
    public Guid Id { get; set; }
    public Guid EventoId { get; set; }
    public Guid ContatoId { get; set; }
    public int Status { get; set; }

    public Evento Evento { get; set; } = null!;
    public Contato Contato { get; set; } = null!;
    public ICollection<PagamentoParticipante>? Pagamento { get; set; }

    public ICollection<ParticipanteListaCusto> ParticipanteListaCustos { get; set; } =
        new List<ParticipanteListaCusto>();
}