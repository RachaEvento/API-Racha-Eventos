using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrganizadorEventos.DTOs.Participantes;
using OrganizadorEventos.Interfaces.Services;
using OrganizadorEventos.Response;

namespace OrganizadorEventos.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ConviteController : ControllerBase
{
    private readonly IConviteService _conviteService;

    public ConviteController(IConviteService conviteService)
    {
        _conviteService = conviteService;
    }
    
    #region Endpoints de gerência do organizador
    
    [HttpPost("evento/{EventoId}/convidar/todos")]
    [Authorize]
    public async Task<IActionResult> ConvidarParticipantesEvento(Guid EventoId)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdClaim, out var userId))
            return Unauthorized(GenericResponse<string>.ErroResponse(new List<string> { "Usuário não encontrado." }));

        await _conviteService.ConvidarTodosParticipantesEvento(EventoId);
        return Ok(GenericResponse<string>.SucessoResponse("","Emails encaminhados"));
    }
    
    [HttpPost("evento/{EventoId}/convidar/{ParticipanteId}")]
    [Authorize]
    public async Task<IActionResult> ConvidarParticipanteEvento(Guid EventoId, Guid ParticipanteId)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdClaim, out var userId))
            return Unauthorized(GenericResponse<string>.ErroResponse(new List<string> { "Usuário não encontrado." }));

        await _conviteService.ConvidarParticipante(ParticipanteId);
        return Ok(GenericResponse<string>.SucessoResponse("","Email encaminhados"));
    }
    
    #endregion 
    
    #region Endpoints de acesso público
    
    [HttpPost("{ParticipanteId}/confirmar")]
    public async Task<IActionResult> ConfirmarParticipante(Guid ParticipanteId)
    {
        await _conviteService.ConfirmarParticipante(ParticipanteId);
        return Ok(GenericResponse<string>.SucessoResponse("","Participante confirmado."));
    }
    
    [HttpPost("{ParticipanteId}/recusar")]
    public async Task<IActionResult> RecusarParticipante(Guid ParticipanteId)
    {
        await _conviteService.RecusarParticipante(ParticipanteId);
        return Ok(GenericResponse<string>.SucessoResponse("","Participante recusado."));
    }
    
    [HttpGet("{ParticipanteId}/convite")]
    public async Task<IActionResult> ConviteParticipante(Guid ParticipanteId)
    {
        var convite = await _conviteService.ConviteParticipante(ParticipanteId);
        return Ok(GenericResponse<ConviteParticipanteDTO>.SucessoResponse(convite,"Convite recuperado com sucesso."));
    }
    
    #endregion
}