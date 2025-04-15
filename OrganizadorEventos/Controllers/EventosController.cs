using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using OrganizadorEventos.Interfaces;
using OrganizadorEventos.Interfaces.Services;
using OrganizadorEventos.Request;
using OrganizadorEventos.Response;

namespace OrganizadorEventos.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EventosController : ControllerBase
{
    private readonly IEventoService _eventoService;

    public EventosController(IEventoService eventoService)
    {
        _eventoService = eventoService;
    }
    
    #region Antigo
    
    /*

    [HttpPost("createEvent")]
    public async Task<IActionResult> CreateEvent([FromBody] CreateEventRequest request)
    {
        if (request == null || string.IsNullOrEmpty(request.Name))
            return BadRequest(GenericResponse<string>.ErroResponse(new List<string>
                { "Dados inválidos para criar um evento." }));

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
            return Unauthorized(GenericResponse<string>.ErroResponse(new List<string> { "Usuário não autenticado." }));

        try
        {
            await _eventoService.CreateEventAsync(request, userId);
            return Ok(GenericResponse<string>.SucessoResponse(null, "Evento criado com sucesso."));
        }
        catch (Exception ex)
        {
            return BadRequest(
                GenericResponse<string>.ErroResponse(new List<string> { ex.Message }, "Erro ao criar evento."));
        }
    }

    [HttpPost("addParticipants/{eventId}")]
    public async Task<IActionResult> AddParticipantsToEvent(Guid eventId, [FromBody] AddParticipantsRequest request)
    {
        if (request.ContactIds == null || request.ContactIds.Count == 0)
            return BadRequest(GenericResponse<string>.ErroResponse(new List<string>
                { "Pelo menos um contato deve ser fornecido." }));

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
            return Unauthorized(GenericResponse<string>.ErroResponse(new List<string> { "Usuário não autenticado." }));

        try
        {
            var updatedEvent = await _eventoService.AddContactsToEventAsync(
                eventId,
                request.ContactIds,
                request.IsPayingParticipants,
                request.IsHalfPriceParticipants,
                userId
            );

            return Ok(GenericResponse<object>.SucessoResponse(updatedEvent, "Participantes adicionados com sucesso."));
        }
        catch (Exception ex)
        {
            return BadRequest(GenericResponse<string>.ErroResponse(new List<string> { ex.Message },
                "Erro ao adicionar participantes."));
        }
    }

    [HttpPost("addCosts/{eventId}")]
    public async Task<IActionResult> AddCost(Guid eventId, [FromBody] AddCostRequest request)
    {
        try
        {
            var cost = await _eventoService.AddCostToEventAsync(eventId, request.Description, request.Amount);
            return Ok(GenericResponse<object>.SucessoResponse(cost, "Custo adicionado com sucesso."));
        }
        catch (Exception ex)
        {
            return BadRequest(GenericResponse<string>.ErroResponse(new List<string> { ex.Message },
                "Erro ao adicionar custo."));
        }
    }

    [HttpGet("getCosts/{eventId}")]
    public async Task<IActionResult> GetCosts(Guid eventId)
    {
        try
        {
            var costs = await _eventoService.GetEventCostsAsync(eventId);
            return Ok(GenericResponse<IEnumerable<object>>.SucessoResponse(costs,
                "Custos do evento recuperados com sucesso."));
        }
        catch (Exception ex)
        {
            return BadRequest(GenericResponse<string>.ErroResponse(new List<string> { ex.Message },
                "Erro ao obter custos do evento."));
        }
    }

    /*
    [HttpPost("finalizeEvent/{eventId}")]
    public async Task<IActionResult> FinalizeEvent(Guid eventId)
    {
        try
        {
            var finalizedEvent = await _eventService.FinalizeEventAsync(eventId);
            return Ok(GenericResponse<object>.SucessoResponse(finalizedEvent, "Evento finalizado com sucesso."));
        }
        catch (Exception ex)
        {
            return BadRequest(GenericResponse<string>.ErroResponse(new List<string> { ex.Message }, "Erro ao finalizar o evento."));
        }
    }
    */
    
    #endregion
}