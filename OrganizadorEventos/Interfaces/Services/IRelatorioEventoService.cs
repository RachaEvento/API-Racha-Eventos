using OrganizadorEventos.DTOs.Relatorio;

namespace OrganizadorEventos.Interfaces.Services;

public interface IRelatorioEventoService
{
    Task<List<CustoParticipanteDTO>> CalcularCustoParticipantesAsync(Guid eventoId); 
}