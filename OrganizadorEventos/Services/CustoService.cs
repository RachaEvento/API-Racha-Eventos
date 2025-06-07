using OrganizadorEventos.DTOs.Custos;
using OrganizadorEventos.Interfaces;
using OrganizadorEventos.Interfaces.Repositories;
using OrganizadorEventos.Mappers;
using OrganizadorEventos.Model;

namespace OrganizadorEventos.Services;

public class CustoService : CrudService<Custo>, ICustoService
{
    private readonly ICustoRepository _custoRepository;
    private readonly IListaCustoRepository _listaCustoRepository;

    public CustoService(ICustoRepository custoRepository, IListaCustoRepository listaCustoRepository) : base(
        custoRepository)
    {
        _custoRepository = custoRepository;
        _listaCustoRepository = listaCustoRepository;
    }

    public async Task<CustoResponseDTO> AdicionarCustoAsync(Guid listaCustoId, AdicionarCustoDTO dto)
    {
        var custo = new Custo
        {
            Id = Guid.NewGuid(),
            ListaCustoId = listaCustoId,
            Nome = dto.Nome,
            Valor = dto.Valor
        };

        await _custoRepository.CreateAsync(custo);
        return custo.ToResponse();
    }

    public async Task RemoverCustoAsync(Guid listaCustoId, RemoverCustoDTO dto)
    {
        await _custoRepository.DeleteAsync(dto.custoId);
    }

    public async Task<List<CustoResponseDTO>> ListarCustosPorListaCustoAsync(Guid listaCustoId)
    {
        var custos = await _custoRepository.GetAllByListaCustoIdAsync(listaCustoId);
        return custos.Select(c => c.ToResponse()).ToList();
    }

    public async Task<List<CustoResponseDTO>> ListarCustosDTOPorParticipanteAsync(Guid participanteId)
    {
        var listasCusto = await _listaCustoRepository.GetAllByParticipanteIdAsync(participanteId);
        var custos = listasCusto.SelectMany(l => l.Custos).ToList();
        return custos.Select(c => c.ToResponse()).ToList();
    }

    public async Task<List<Custo>> ListarCustosPorParticipanteAsync(Guid participanteId)
    {
        var listasCusto = await _listaCustoRepository.GetAllByParticipanteIdAsync(participanteId);
        return listasCusto.SelectMany(l => l.Custos).ToList();
    }

    public async Task<List<Participante>> ListarParticipantesPorCustoAsync(Guid custoId)
    {
        var custo = await _custoRepository.GetByIdAsync(custoId);

        if (custo == null)
            return new List<Participante>();

        return custo.ListaCusto.ParticipanteListaCustos.Select(p => p.Participante).ToList();
    }
}