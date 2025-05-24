using OrganizadorEventos.Enum;
using OrganizadorEventos.Request;
using OrganizadorEventos.Request.Evento;

namespace OrganizadorEventos.DTOs.Participantes;

public class ConviteParticipanteDTO
{
    public ParticipanteDTO contatoParticipante { get; set; }
    public ListarEventosDTO evento { get; set; }
    public StatusParticipante status { get; set; }
}