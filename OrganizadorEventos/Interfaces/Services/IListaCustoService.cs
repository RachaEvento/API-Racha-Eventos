using OrganizadorEventos.DTOs.Custos;
using OrganizadorEventos.Response;

namespace OrganizadorEventos.Interfaces.Services;

public interface IListaCustoService
{
    Task<GenericResponse<string>> CriarListaCustoAsync(CriarListaCustoDTO dto);
    Task<List<ListaCustoResponseDTO>> ListarListasDeCustoPorEventoAsync(Guid eventoId);
}