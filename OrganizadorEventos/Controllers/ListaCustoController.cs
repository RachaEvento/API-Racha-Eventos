using Microsoft.AspNetCore.Mvc;
using OrganizadorEventos.DTOs.Custos;
using OrganizadorEventos.DTOs.ListaCusto;
using OrganizadorEventos.Interfaces.Services;
using OrganizadorEventos.Request.Evento;
using OrganizadorEventos.Response;

namespace OrganizadorEventos.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ListaCustoController : ControllerBase
{
    private readonly IListaCustoService _listaCustoService;
    private readonly IParticipanteListaCustoService _participanteListaCustoService;

    public ListaCustoController(
        IListaCustoService listaCustoService,
        IParticipanteListaCustoService participanteListaCustoService)
    {
        _listaCustoService = listaCustoService;
        _participanteListaCustoService = participanteListaCustoService;
    }

    [HttpGet("evento/{eventoId}")]
    public async Task<ActionResult<GenericResponse<List<ListaCustosComCustosEParticipantesDto>>>>
        ListarListasCustoPorEvento(Guid eventoId)
    {
        try
        {
            var listas = await _listaCustoService.ListarListaCustosComCustosEParticipantesAsync(eventoId);
            return Ok(GenericResponse<List<ListaCustosComCustosEParticipantesDto>>.SucessoResponse(listas));
        }
        catch (Exception ex)
        {
            return BadRequest(GenericResponse<string>.ErroResponse(new List<string> { ex.Message },
                "Erro ao listar listas de custos do evento."));
        }
    }

    [HttpPost("evento/{eventoId}/criar")]
    public async Task<ActionResult<GenericResponse<string>>> CriarListaCusto(Guid eventoId,
        [FromBody] CriarListaCustoDTO dto)
    {
        try
        {
            var response = await _listaCustoService.CriarListaCustoAsync(eventoId, dto);
            return Ok(GenericResponse<string>.SucessoResponse(response));
        }
        catch (Exception ex)
        {
            return BadRequest(GenericResponse<string>.ErroResponse(new List<string> { ex.Message },
                "Erro ao criar lista de custos."));
        }
    }

    [HttpPost("{listaCustoId}/participantes/adicionar")]
    public async Task<ActionResult<GenericResponse<string>>> AdicionarParticipantes(Guid listaCustoId,
        [FromBody] AdicionarParticipantesListaCustoDTO dto)
    {
        try
        {
            await _participanteListaCustoService.AdicionarParticipantesAsync(listaCustoId, dto);
            return Ok(GenericResponse<string>.SucessoResponse(null, "Participantes adicionados com sucesso."));
        }
        catch (Exception ex)
        {
            return BadRequest(GenericResponse<string>.ErroResponse(new List<string> { ex.Message },
                "Erro ao adicionar participante na lista de custo"));
        }
    }

    [HttpPost("{listaCustoId}/participantes/remover")]
    public async Task<ActionResult<GenericResponse<string>>> RemoverParticipantes(Guid listaCustoId,
        [FromBody] RemoverParticipantesListaCustoDTO dto)
    {
        try
        {
            await _participanteListaCustoService.RemoverParticipantesAsync(listaCustoId, dto);
            return Ok(GenericResponse<string>.SucessoResponse(null, "Participantes removidos com sucesso."));
        }
        catch (Exception ex)
        {
            return BadRequest(GenericResponse<string>.ErroResponse(new List<string> { ex.Message },
                "Erro ao remover participante na lista de custo"));
        }
    }
}