using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrganizadorEventos.Interfaces.Services;
using OrganizadorEventos.Mappers;
using OrganizadorEventos.Request;
using OrganizadorEventos.Request.Evento;
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
    
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<GenericResponse<IEnumerable<ListarEventosDTO>>>> GetAll()
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!Guid.TryParse(userIdClaim, out var userId))
            return Unauthorized(GenericResponse<string>.ErroResponse(new List<string> { "Usuário não encontrado." }));

        var eventos = await _eventoService.GetAllByUserAsync(userId);
        var eventoRequest = eventos.Select(e => e.ToRequest()).ToList();

        return Ok(GenericResponse<IEnumerable<ListarEventosDTO>>.SucessoResponse(eventoRequest,"Eventos carregados com sucesso."));
    }
    
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<GenericResponse<CriarEventoDTO>>> Create([FromBody] CriarEventoDTO dto)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdClaim, out var userId))
            return Unauthorized(GenericResponse<string>.ErroResponse(new List<string> { "Usuário não encontrado." }));

        var entity = await _eventoService.CreateAsync(dto.ToEntity(userId), dto.ContatosParticipantes);
        return Ok(GenericResponse<ListarEventosDTO>.SucessoResponse(entity.ToRequest(),"Evento criado com sucesso."));
    }
    
    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<GenericResponse<ListarEventosDTO>>> GetById(Guid id)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdClaim, out var userId))
            return Unauthorized(GenericResponse<string>.ErroResponse(new List<string> { "Usuário não encontrado." }));

        var evento = await _eventoService.GetByIdAsync(id);
        return Ok(GenericResponse<ListarEventosDTO>.SucessoResponse(evento.ToRequest(),"Evento carregado com sucesso."));
    }
    
    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> Update(Guid id, [FromBody] EditarEventoDTO dto)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdClaim, out var userId))
            return Unauthorized(GenericResponse<string>.ErroResponse(new List<string> { "Usuário não encontrado." }));
        
        await _eventoService.UpdateAsync(dto, id);
        return Ok(GenericResponse<string>.SucessoResponse("","Evento atualizado com sucesso."));
    }
    
    [HttpGet("{id}/participantes")]
    [Authorize]
    public async Task<IActionResult> ListParticipants(Guid id)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdClaim, out var userId))
            return Unauthorized(GenericResponse<string>.ErroResponse(new List<string> { "Usuário não encontrado." }));
        
        var participantes = await _eventoService.ListarParticipantes(id);
        return Ok(GenericResponse<List<ContatoDTO>>.SucessoResponse(participantes,"Participantes recuperados com sucesso."));
    }
    
    [HttpPut("{id}/participantes/adicionar")]
    [Authorize]
    public async Task<IActionResult> AddParticipants(Guid id, [FromBody] List<Guid> participantes)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdClaim, out var userId))
            return Unauthorized(GenericResponse<string>.ErroResponse(new List<string> { "Usuário não encontrado." }));
        
        await _eventoService.AdicionarParticipantesAsync(participantes, id);
        return Ok(GenericResponse<string>.SucessoResponse("","Participantes adicionados com sucesso."));
    }
    
    [HttpPut("{id}/participantes/remover")]
    [Authorize]
    public async Task<IActionResult> RemoveParticipants(Guid id, [FromBody] List<Guid> participantes)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdClaim, out var userId))
            return Unauthorized(GenericResponse<string>.ErroResponse(new List<string> { "Usuário não encontrado." }));
        
        await _eventoService.RemoverParticipantesAsync(participantes, id);
        return Ok(GenericResponse<string>.SucessoResponse("","Participantes removidos com sucesso."));
    }
}