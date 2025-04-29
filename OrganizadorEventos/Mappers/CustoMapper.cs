using OrganizadorEventos.DTOs.Custos;
using OrganizadorEventos.Model;

namespace OrganizadorEventos.Mappers;

public static class CustoMapper
{
    public static CustoResponseDTO ToResponse(this Custo custo)
    {
        return new CustoResponseDTO
        {
            Id = custo.Id,
            Nome = custo.Nome,
            Valor = custo.Valor
        };
    }
}