namespace OrganizadorEventos.Request.Evento;

public class ListaCustosComCustosEParticipantesDto
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public List<CustoDTO> Custos { get; set; }
    public List<ContatoDTO> Participantes { get; set; }
}