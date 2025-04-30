namespace OrganizadorEventos.DTOs.Email;

public class ConviteEmailDTO
{
    public string ToEmail { get; set; }
    public string ConvidadoNome { get; set; }
    public string QuemConvidaNome { get; set; }
    public string EventoNome { get; set; }
    public DateTime EventoData { get; set; }
    public string CodigoConfirmacao { get; set; }
}