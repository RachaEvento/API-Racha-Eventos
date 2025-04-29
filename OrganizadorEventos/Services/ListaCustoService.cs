using OrganizadorEventos.DTOs.Custos;
using OrganizadorEventos.Interfaces.Repositories;
using OrganizadorEventos.Interfaces.Services;
using OrganizadorEventos.Mappers;
using OrganizadorEventos.Model;
using OrganizadorEventos.Response;

namespace OrganizadorEventos.Services;

public class ListaCustoService : IListaCustoService
{
    private readonly IListaCustoRepository _listaCustoRepository;


    public ListaCustoService(
        IListaCustoRepository listaCustoRepository
    )
    {
        _listaCustoRepository = listaCustoRepository;
    }

    public async Task<GenericResponse<string>> CriarListaCustoAsync(CriarListaCustoDTO dto)
    {
        var listaCusto = new ListaCusto
        {
            Id = Guid.NewGuid(),
            EventoId = dto.EventoId,
            Nome = dto.Nome
        };

        await _listaCustoRepository.CreateAsync(listaCusto);

        return GenericResponse<string>.SucessoResponse("Lista de custo criada com sucesso.");
    }


    public async Task<List<ListaCustoResponseDTO>> ListarListasDeCustoPorEventoAsync(Guid eventoId)
    {
        var listasCusto = await _listaCustoRepository.GetAllByEventoIdAsync(eventoId);
        return listasCusto.ToResponse();
    }
}