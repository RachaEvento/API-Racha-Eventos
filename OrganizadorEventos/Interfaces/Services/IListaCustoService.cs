using OrganizadorEventos.DTOs.Custos;
using OrganizadorEventos.DTOs.ListaCusto;
using OrganizadorEventos.Request.Evento;
using OrganizadorEventos.Response;

namespace OrganizadorEventos.Interfaces.Services;

public interface IListaCustoService
{
    Task<string> CriarListaCustoAsync(Guid eventoId, CriarListaCustoDTO dto);
    Task<List<ListaCustosComCustosEParticipantesDto>> ListarListaCustosComCustosEParticipantesAsync(Guid eventoId);

}