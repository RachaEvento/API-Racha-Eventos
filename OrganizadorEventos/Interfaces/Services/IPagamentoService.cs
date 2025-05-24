using OrganizadorEventos.DTOs.Participantes;
using OrganizadorEventos.Enum;

namespace OrganizadorEventos.Interfaces.Services;

public interface IPagamentoService
{
    Task<InformacoesPagamentoDTO> InformacoesPagamento(Guid participanteId);
    Task SalvarPagamento(Guid participanteId, string fileId);
    Task<ListaPagamentosDTO> ListarPagamentoParticipante(Guid participanteId);
    Task AlterarPagamentoParticipante(Guid pagamentoId, StatusPagamento status);
}