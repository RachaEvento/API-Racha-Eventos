using OrganizadorEventos.DTOs.Custos;
using OrganizadorEventos.Model;

namespace OrganizadorEventos.Mappers;

public static class ListaCustoMapper
{
    // Converte ListaCusto para ListaCustoResponseDTO
    public static ListaCustoResponseDTO ToResponse(this ListaCusto listaCusto)
    {
        return new ListaCustoResponseDTO
        {
            Id = listaCusto.Id,
            Nome = listaCusto.Nome
        };
    }

    // Converte uma lista de ListaCusto para uma lista de ListaCustoResponseDTO
    public static List<ListaCustoResponseDTO> ToResponse(this List<ListaCusto> listaCustos)
    {
        return listaCustos.Select(x => x.ToResponse()).ToList();
    }
}