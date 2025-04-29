using Microsoft.AspNetCore.Mvc;
using OrganizadorEventos.DTOs.Custos;
using OrganizadorEventos.Interfaces;
using OrganizadorEventos.Response;

namespace OrganizadorEventos.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustoController : ControllerBase
{
    private readonly ICustoService _custoService;

    public CustoController(ICustoService custoService)
    {
        _custoService = custoService;
    }

    [HttpPost]
    public async Task<ActionResult<GenericResponse<Guid>>> AdicionarCusto([FromBody] AdicionarCustoDTO dto)
    {
        var id = await _custoService.AdicionarCustoAsync(dto);
        return Ok(GenericResponse<Guid>.SucessoResponse(id, "Custo adicionado com sucesso."));
    }

    [HttpGet("listacusto/{listaCustoId}")]
    public async Task<ActionResult<GenericResponse<List<CustoResponseDTO>>>> ListarCustosPorLista(Guid listaCustoId)
    {
        var custos = await _custoService.ListarCustosPorListaCustoAsync(listaCustoId);
        return Ok(GenericResponse<List<CustoResponseDTO>>.SucessoResponse(custos));
    }
}