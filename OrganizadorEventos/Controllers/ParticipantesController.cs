using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrganizadorEventos.Interfaces.Services;
using OrganizadorEventos.Request;
using OrganizadorEventos.Response;

namespace OrganizadorEventos.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ParticipantesController: ControllerBase
{
    private readonly IParticipanteService _participanteService;

    public ParticipantesController(IParticipanteService participanteService)
    {
        _participanteService = participanteService;
    }
    
    [HttpGet("{EventoId}")]
    [Authorize]
    public async Task<IActionResult> ListParticipants(Guid EventoId)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdClaim, out var userId))
            return Unauthorized(GenericResponse<string>.ErroResponse(new List<string> { "Usuário não encontrado." }));
        
        var participantes = await _participanteService.ListarParticipantes(EventoId);
        return Ok(GenericResponse<List<ContatoDTO>>.SucessoResponse(participantes,"Participantes recuperados com sucesso."));
    }
    
    [HttpPost("{EventoId}/adicionar")]
    [Authorize]
    public async Task<IActionResult> AddParticipants(Guid EventoId, [FromBody] List<Guid> participantes)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdClaim, out var userId))
            return Unauthorized(GenericResponse<string>.ErroResponse(new List<string> { "Usuário não encontrado." }));
        
        await _participanteService.AdicionarParticipantesAsync(participantes, EventoId);
        return Ok(GenericResponse<string>.SucessoResponse("","Participantes adicionados com sucesso."));
    }
    
    [HttpPost("{EventoId}/remover")]
    [Authorize]
    public async Task<IActionResult> RemoveParticipants(Guid EventoId, [FromBody] List<Guid> participantes)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdClaim, out var userId))
            return Unauthorized(GenericResponse<string>.ErroResponse(new List<string> { "Usuário não encontrado." }));
        
        await _participanteService.RemoverParticipantesAsync(participantes, EventoId);
        return Ok(GenericResponse<string>.SucessoResponse("","Participantes removidos com sucesso."));
    }
}