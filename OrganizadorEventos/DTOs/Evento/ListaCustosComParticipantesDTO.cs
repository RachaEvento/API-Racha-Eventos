namespace OrganizadorEventos.Request.Evento;

public class ListaCustosComParticipantesDTO
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public List<CustoDTO> Custo { get; set; }
}