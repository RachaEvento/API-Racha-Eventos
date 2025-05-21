namespace OrganizadorEventos.Model;

public class PagamentoParticipante
{
    public Guid Id { get; set; }
    public Guid ParticipanteId { get; set; }
    public string Comprovante { get; set; } = null!;
    public int Status { get; set; }
    public DateTime DataPagamento { get; set; } = DateTime.UtcNow;

    public Participante Participante { get; set; } = null!;
}