using OrganizadorEventos.DTOs.Custos;
using OrganizadorEventos.Model;

namespace OrganizadorEventos.Interfaces;

public interface ICustoService : ICrudService<Custo>
{
    Task<CustoResponseDTO> AdicionarCustoAsync(Guid listaCustoId, AdicionarCustoDTO dto);
    Task RemoverCustoAsync(Guid listaCustoId, RemoverCustoDTO dto);
    Task<List<CustoResponseDTO>> ListarCustosPorListaCustoAsync(Guid listaCustoId);
    Task<List<CustoResponseDTO>> ListarCustosDTOPorParticipanteAsync(Guid participanteId);
    Task<List<Custo>> ListarCustosPorParticipanteAsync(Guid participanteId);
    Task<List<Participante>> ListarParticipantesPorCustoAsync(Guid custoId);
}