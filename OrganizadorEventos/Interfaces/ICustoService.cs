using OrganizadorEventos.DTOs.Custos;
using OrganizadorEventos.Model;

namespace OrganizadorEventos.Interfaces;

public interface ICustoService : ICrudService<Custo>
{
    Task<CustoResponseDTO> AdicionarCustoAsync(Guid listaCustoId, AdicionarCustoDTO dto);
    Task RemoverCustoAsync(Guid listaCustoId, RemoverCustoDTO dto);
    Task<List<CustoResponseDTO>> ListarCustosPorListaCustoAsync(Guid listaCustoId);
    Task<List<CustoResponseDTO>> ListarCustosPorParticipanteAsync(Guid participanteId);
}