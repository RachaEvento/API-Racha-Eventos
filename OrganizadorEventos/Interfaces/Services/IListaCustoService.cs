using OrganizadorEventos.DTOs.Custos;
using OrganizadorEventos.Request.Evento;
using OrganizadorEventos.Response;

namespace OrganizadorEventos.Interfaces.Services;

public interface IListaCustoService
{
    Task<GenericResponse<string>> CriarListaCustoAsync(CriarListaCustoDTO dto);
    Task<List<ListaCustoResponseDTO>> ListarListasDeCustoPorEventoAsync(Guid eventoId);
    Task<List<EventoComCustosDto>> ListarEventosComCustosAsync(Guid eventoId);

}