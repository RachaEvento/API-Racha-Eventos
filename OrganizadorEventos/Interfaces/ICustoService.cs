using OrganizadorEventos.DTOs.Custos;

namespace OrganizadorEventos.Interfaces;

public interface ICustoService
{
    Task<Guid> AdicionarCustoAsync(AdicionarCustoDTO dto);
    Task<List<CustoResponseDTO>> ListarCustosPorListaCustoAsync(Guid listaCustoId);
}