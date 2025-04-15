namespace OrganizadorEventos.Model;

public class ParticipanteListaCusto
{
    public Guid ListaCustoId { get; set; }
    public Guid ParticipanteId { get; set; }

    public ListaCusto ListaCusto { get; set; } = null!;
    public Participante Participante { get; set; } = null!;
}