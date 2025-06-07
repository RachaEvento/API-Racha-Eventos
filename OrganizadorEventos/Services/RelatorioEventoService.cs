using OrganizadorEventos.DTOs.Relatorio;
using OrganizadorEventos.Enum;
using OrganizadorEventos.Interfaces;
using OrganizadorEventos.Interfaces.Repositories;
using OrganizadorEventos.Interfaces.Services;
using OrganizadorEventos.Mappers;
using OrganizadorEventos.Model;

namespace OrganizadorEventos.Services;

public class RelatorioEventoService : IRelatorioEventoService
{
    private readonly ICustoService _custoService;
    private readonly IParticipanteRepository _participanteRepository;

    public RelatorioEventoService(IParticipanteRepository participanteRepository, ICustoService custoService)
    {
        _participanteRepository = participanteRepository;
        _custoService = custoService;
    }

    public async Task<List<CustoParticipanteDTO>> CalcularCustoParticipantesAsync(Guid eventoId)
    {
        var participantes = await _participanteRepository.GetAllConfirmedByEventIdAsync(eventoId);
        var participantesPorId = participantes.ToDictionary(p => p.Id, p =>
        {
            var dto = p.Contato.ToRequest();
            dto.Id = p.Id;
            return dto;
        });

        var custosPorParticipante = new Dictionary<Guid, List<Guid>>();

        // Busca todos os custos de todos os participantes
        foreach (var participante in participantes)
        {
            var custos = await _custoService.ListarCustosDTOPorParticipanteAsync(participante.Id);
            foreach (var custo in custos)
            {
                if (!custosPorParticipante.ContainsKey(custo.Id))
                    custosPorParticipante[custo.Id] = new List<Guid>();

                if (!custosPorParticipante[custo.Id].Contains(participante.Id))
                    custosPorParticipante[custo.Id].Add(participante.Id);
            }
        }

        // Dicionário para acumular o custo de cada participante
        var custoAcumuladoPorParticipante = participantes.ToDictionary(p => p.Id, p => 0m);

        // Agora somamos os custos e dividimos proporcionalmente
        foreach (var (custoId, listaParticipantes) in custosPorParticipante)
        {
            var custo = await _custoService.GetByIdAsync(custoId); // busca valor total desse custo
            var valorPorParticipante = custo.Valor / listaParticipantes.Count;

            foreach (var participanteId in listaParticipantes)
                custoAcumuladoPorParticipante[participanteId] += valorPorParticipante;
        }

        // Monta a lista de retorno
        var resultado = custoAcumuladoPorParticipante.Select(kvp => new CustoParticipanteDTO
        {
            Participante = participantesPorId[kvp.Key],
            Custo = kvp.Value
        }).ToList();

        return resultado;
    }

    public async Task<CustoParticipanteDTO> CalcularCustoParticipanteAsync(Participante participante)
    {
        if (participante == null || participante.Status != (int)StatusParticipante.Confirmado)
            return null;

        var custoTotal = 0m;
        var custos = await _custoService.ListarCustosPorParticipanteAsync(participante.Id);
        foreach (var custo in custos)
        {
            var totalDesseCusto = custo.Valor / custo.ListaCusto.ParticipanteListaCustos.Count;
            custoTotal += totalDesseCusto;
        }

        return new CustoParticipanteDTO
        {
            Participante = participante.Contato.ToRequest(),
            Custo = custoTotal
        };
    }
}