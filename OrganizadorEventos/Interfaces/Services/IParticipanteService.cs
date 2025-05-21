using OrganizadorEventos.DTOs.Participantes;
using OrganizadorEventos.Enum;
using OrganizadorEventos.Request;

namespace OrganizadorEventos.Interfaces.Services;

public interface IParticipanteService
{
    Task AdicionarContatosComoParticipantesAsync(AdicionarParticipanteDTO contatos, Guid eventoId);
    Task RemoverParticipantesAsync(RemoverParticipanteDTO participantes, Guid eventoId);
    Task<List<ParticipanteDTO>> ListarParticipantes(Guid eventoId);
    Task ConvidarTodosParticipantesEvento(Guid eventoId);
    Task ConvidarParticipante(Guid participanteId);
    Task ConfirmarParticipante(Guid participanteId);
    Task RecusarParticipante(Guid participanteId);
    Task<ConviteParticipanteDTO> ConviteParticipante(Guid participanteId);
    Task<InformacoesPagamentoDTO> InformacoesPagamento(Guid participanteId);
    Task SalvarPagamento(Guid participanteId, string fileId);
    Task<ListaPagamentosDTO> ListarPagamentoParticipante(Guid participanteId);
    Task AlterarPagamentoParticipante(Guid pagamentoId, StatusPagamento status);
}