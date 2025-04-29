using OrganizadorEventos.DTOs.Custos;
using OrganizadorEventos.Interfaces;
using OrganizadorEventos.Interfaces.Repositories;
using OrganizadorEventos.Mappers;
using OrganizadorEventos.Model;

namespace OrganizadorEventos.Services;

public class CustoService : ICustoService
{
    private readonly ICustoRepository _custoRepository;

    public CustoService(ICustoRepository custoRepository)
    {
        _custoRepository = custoRepository;
    }

    public async Task<Guid> AdicionarCustoAsync(AdicionarCustoDTO dto)
    {
        var custo = new Custo
        {
            Id = Guid.NewGuid(),
            ListaCustoId = dto.ListaCustoId,
            Nome = dto.Nome,
            Valor = dto.Valor
        };

        await _custoRepository.CreateAsync(custo);
        return custo.Id;
    }

    public async Task<List<CustoResponseDTO>> ListarCustosPorListaCustoAsync(Guid listaCustoId)
    {
        var custos = await _custoRepository.GetAllByListaCustoIdAsync(listaCustoId);
        return custos.Select(c => c.ToResponse()).ToList();
    }
}