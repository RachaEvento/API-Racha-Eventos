using OrganizadorEventos.Enum;
using OrganizadorEventos.Model;

namespace OrganizadorEventos.Request.Evento;

public class CriarEventoDTO
{
    public string  Nome { get; set; } = null!;
    public string? Descricao { get; set; }
    public DateTime DataInicio { get; set; }
    public DateTime? DataFinal { get; set; }
    public StatusEvento Status { get; set; }
    public Guid? LocalId { get; set; }
    public List<Guid>? ContatosParticipantes { get; set; }
}