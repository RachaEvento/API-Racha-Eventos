using OrganizadorEventos.DTOs.ListaCusto;
using OrganizadorEventos.Request.Evento;

namespace OrganizadorEventos.Interfaces.Services;

public interface IListaCustoService
{
    Task<string> CriarListaCustoAsync(Guid eventoId, CriarListaCustoDTO dto);
    Task<List<ListaCustosComCustosEParticipantesDto>> ListarListaCustosComCustosEParticipantesAsync(Guid eventoId);
}