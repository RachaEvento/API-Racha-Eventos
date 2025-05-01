using OrganizadorEventos.Request;

namespace OrganizadorEventos.DTOs.Relatorio;

public class CustoParticipanteDTO
{
    public ContatoDTO Participante { get; set; }
    public decimal Custo { get; set; }
}