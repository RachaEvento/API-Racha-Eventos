using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrganizadorEventos.DTOs.Email;
using OrganizadorEventos.DTOs.Participantes;
using OrganizadorEventos.Interfaces.Services;
using OrganizadorEventos.Request;
using OrganizadorEventos.Response;

namespace OrganizadorEventos.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ParticipantesController : ControllerBase
{
    private readonly IParticipanteService _participanteService;

    public ParticipantesController(IParticipanteService participanteService)
    {
        _participanteService = participanteService;
    }
    
    [HttpGet("evento/{EventoId}")]
    [Authorize]
    public async Task<IActionResult> ListParticipants(Guid EventoId)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdClaim, out var userId))
            return Unauthorized(GenericResponse<string>.ErroResponse(new List<string> { "Usuário não encontrado." }));

        var participantes = await _participanteService.ListarParticipantes(EventoId);
        return Ok(GenericResponse<List<ContatoDTO>>.SucessoResponse(participantes,
            "Participantes recuperados com sucesso."));
    }
    
    [HttpPost("evento/{EventoId}/adicionar")]
    [Authorize]
    public async Task<IActionResult> AddParticipants(Guid EventoId, [FromBody] AdicionarParticipanteDTO contatos)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdClaim, out var userId))
            return Unauthorized(GenericResponse<string>.ErroResponse(new List<string> { "Usuário não encontrado." }));
        
        await _participanteService.AdicionarContatosComoParticipantesAsync(contatos, EventoId);
        return Ok(GenericResponse<string>.SucessoResponse("","Participantes adicionados com sucesso."));
    }
    
    [HttpPost("evento/{EventoId}/remover")]
    [Authorize]
    public async Task<IActionResult> RemoveParticipants(Guid EventoId, [FromBody] RemoverParticipanteDTO participantes)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdClaim, out var userId))
            return Unauthorized(GenericResponse<string>.ErroResponse(new List<string> { "Usuário não encontrado." }));

        await _participanteService.RemoverParticipantesAsync(participantes, EventoId);
        return Ok(GenericResponse<string>.SucessoResponse("", "Participantes removidos com sucesso."));
    }
    
    [HttpPost("evento/{EventoId}/convidar/todos")]
    [Authorize]
    public async Task<IActionResult> ConvidarParticipantesEvento(Guid EventoId)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdClaim, out var userId))
            return Unauthorized(GenericResponse<string>.ErroResponse(new List<string> { "Usuário não encontrado." }));

        await _participanteService.ConvidarTodosParticipantesEvento(EventoId);
        return Ok(GenericResponse<string>.SucessoResponse("","Emails encaminhados"));
    }
    
    [HttpPost("evento/{EventoId}/convidar/{ParticipanteId}")]
    [Authorize]
    public async Task<IActionResult> ConvidarParticipanteEvento(Guid EventoId, Guid ParticipanteId)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdClaim, out var userId))
            return Unauthorized(GenericResponse<string>.ErroResponse(new List<string> { "Usuário não encontrado." }));

        await _participanteService.ConvidarParticipante(ParticipanteId);
        return Ok(GenericResponse<string>.SucessoResponse("","Email encaminhados"));
    }
    
    [HttpPost("{ParticipanteId}/confirmar")]
    [Authorize]
    public async Task<IActionResult> ConfirmarParticipante(Guid ParticipanteId)
    {
        await _participanteService.ConfirmarParticipante(ParticipanteId);
        return Ok(GenericResponse<string>.SucessoResponse("","Participante confirmado."));
    }
    
    [HttpPost("{ParticipanteId}/recusar")]
    [Authorize]
    public async Task<IActionResult> RecusarParticipante(Guid ParticipanteId)
    {
        await _participanteService.RecusarParticipante(ParticipanteId);
        return Ok(GenericResponse<string>.SucessoResponse("","Participante recusado."));
    }
    
    [HttpGet("{ParticipanteId}/convite")]
    [Authorize]
    public async Task<IActionResult> ConviteParticipante(Guid ParticipanteId)
    {
        var convite = await _participanteService.ConviteParticipante(ParticipanteId);
        return Ok(GenericResponse<ConviteParticipanteDTO>.SucessoResponse(convite,"Convite recuperado com sucesso."));
    }
    
    [HttpGet("{ParticipanteId}/pagamento")]
    [Authorize]
    public async Task<IActionResult> InformacoesPagamento(Guid ParticipanteId)
    {
        var informacoes = await _participanteService.InformacoesPagamento(ParticipanteId);
        return Ok(GenericResponse<InformacoesPagamentoDTO>.SucessoResponse(informacoes,"Informações recuperadas com sucesso."));
    }
}