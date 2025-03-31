using Microsoft.AspNetCore.Mvc;
using OrganizadorEventos.Interfaces;
using OrganizadorEventos.Request;

namespace OrganizadorEventos.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EventController : ControllerBase
{
    private readonly IEventService _eventService;

    public EventController(IEventService eventService)
    {
        _eventService = eventService;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateEvent([FromBody] CreateEventRequest request)
    {
        if (request == null || string.IsNullOrEmpty(request.Name))
            return BadRequest("Dados inválidos para criar um evento.");

        try
        {
            await _eventService.CreateEventAsync(request);
            return Ok("Evento Criado com sucesso");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("addParticipants/{eventId}")]
    public async Task<IActionResult> AddParticipantsToEvent(Guid eventId, [FromBody] AddParticipantsRequest request)
    {
        if (request.ContactIds == null || request.ContactIds.Count == 0)
            return BadRequest("Pelo menos um contato deve ser fornecido.");

        try
        {
            var updatedEvent = await _eventService.AddContactsToEventAsync(eventId, request.ContactIds,
                request.IsPayingParticipants, request.IsHalfPriceParticipants);
            return Ok(updatedEvent);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("finalizeEvent/{eventId}")]
    public async Task<IActionResult> FinalizeEvent(Guid eventId)
    {
        try
        {
            var finalizedEvent = await _eventService.FinalizeEventAsync(eventId);
            return Ok(finalizedEvent);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("addCosts/{eventId}")]
    public async Task<IActionResult> AddCost(Guid eventId, [FromBody] AddCostRequest request)
    {
        var cost = await _eventService.AddCostToEventAsync(eventId, request.Description, request.Amount);
        return Ok(cost);
    }

    [HttpGet("getCosts/{eventId}")]
    public async Task<IActionResult> GetCosts(Guid eventId)
    {
        var costs = await _eventService.GetEventCostsAsync(eventId);
        return Ok(costs);
    }
}