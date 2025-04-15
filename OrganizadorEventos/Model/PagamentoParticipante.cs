namespace OrganizadorEventos.Model;

public class PagamentoParticipante
{
    public Guid Id { get; set; }
    public Guid ParticipanteId { get; set; }
    public string Comprovante { get; set; } = null!;
    public bool? Aceito { get; set; }

    public Participante Participante { get; set; } = null!;
}