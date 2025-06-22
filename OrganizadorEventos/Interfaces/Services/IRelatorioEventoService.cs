using OrganizadorEventos.DTOs.Evento;
using OrganizadorEventos.DTOs.Participantes;
using OrganizadorEventos.DTOs.Relatorio;
using OrganizadorEventos.Model;

namespace OrganizadorEventos.Interfaces.Services;

public interface IRelatorioEventoService
{
    Task<List<CustoParticipanteDTO>> CalcularCustoParticipantesAsync(Guid eventoId);
    Task<CustoParticipanteDTO> CalcularCustoParticipanteAsync(Participante participante);
    Task<CustoParticipanteDTO> CalcularCustoParticipanteAsync(ParticipanteDTO participante);
    Task<ResultadoRelatorioEvento> GerarRelatorioEventoAsync(Guid eventoId);
}