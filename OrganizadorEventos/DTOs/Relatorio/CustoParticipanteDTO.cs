using OrganizadorEventos.DTOs.Participantes;
using OrganizadorEventos.Request;

namespace OrganizadorEventos.DTOs.Relatorio;

public class CustoParticipanteDTO
{
    public ParticipanteDTO Participante { get; set; }
    public decimal Custo { get; set; }
}