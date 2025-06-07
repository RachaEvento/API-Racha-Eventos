namespace OrganizadorEventos.DTOs.Email;

public class CobrancaEmailDTO
{
    public string ToEmail { get; set; }
    public string ConvidadoNome { get; set; }
    public string QuemConvidaNome { get; set; }
    public string EventoNome { get; set; }
    public decimal Valor { get; set; }
    public string CodigoParticipante { get; set; }
}