using Microsoft.AspNetCore.Mvc;
using OrganizadorEventos.DTOs.Custos;
using OrganizadorEventos.Interfaces;
using OrganizadorEventos.Model;
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

    [HttpGet("listacusto/{listaCustoId}")]
    public async Task<ActionResult<GenericResponse<List<CustoResponseDTO>>>> ListarCustosPorLista(Guid listaCustoId)
    {
        var custos = await _custoService.ListarCustosPorListaCustoAsync(listaCustoId);
        return Ok(GenericResponse<List<CustoResponseDTO>>.SucessoResponse(custos));
    }
    
    [HttpPost("listacusto/{listaCustoId}/adicionar")]
    public async Task<ActionResult<GenericResponse<CustoResponseDTO>>> AdicionarCusto(Guid listaCustoId, [FromBody] AdicionarCustoDTO dto)
    {
        try
        {
            var custo = await _custoService.AdicionarCustoAsync(listaCustoId, dto);
            return Ok(GenericResponse<CustoResponseDTO>.SucessoResponse(custo, "Custo adicionado com sucesso."));
        }
        catch (Exception ex)
        {
            return BadRequest(GenericResponse<string>.ErroResponse(new List<string>(){ex.Message}, "Erro ao adicionar custo."));
        }
    }
    
    [HttpPost("listacusto/{listaCustoId}/remover")]
    public async Task<ActionResult<GenericResponse<Guid>>> RemoverCusto(Guid listaCustoId, [FromBody] RemoverCustoDTO custoId)
    {
        try
        {
            await _custoService.RemoverCustoAsync(listaCustoId, custoId);
            return Ok(GenericResponse<string>.SucessoResponse("","Custo removido com sucesso."));
        }
        catch (Exception ex)
        {
            return BadRequest(GenericResponse<string>.ErroResponse(new List<string>(){ex.Message}, "Erro ao remover custo."));
        }
    }
}