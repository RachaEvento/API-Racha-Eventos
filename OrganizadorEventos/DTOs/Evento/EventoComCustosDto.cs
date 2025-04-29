namespace OrganizadorEventos.Request.Evento;

public class EventoComCustosDto
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public List<CustoDto> Custo { get; set; }
}
public class CustoDto
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public decimal Valor { get; set; }
}