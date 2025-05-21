using OrganizadorEventos.DTOs.Relatorio;
using OrganizadorEventos.Model;

namespace OrganizadorEventos.Interfaces.Services;

public interface IRelatorioEventoService
{
    Task<List<CustoParticipanteDTO>> CalcularCustoParticipantesAsync(Guid eventoId);
    Task<CustoParticipanteDTO> CalcularCustoParticipanteAsync(Participante participante);
}