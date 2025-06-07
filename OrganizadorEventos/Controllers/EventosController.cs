using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrganizadorEventos.Enum;
using OrganizadorEventos.Interfaces.Services;
using OrganizadorEventos.Mappers;
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

        return Ok(GenericResponse<IEnumerable<ListarEventosDTO>>.SucessoResponse(eventoRequest,
            "Eventos carregados com sucesso."));
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<GenericResponse<CriarEventoDTO>>> Create([FromBody] CriarEventoDTO dto)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdClaim, out var userId))
            return Unauthorized(GenericResponse<string>.ErroResponse(new List<string> { "Usuário não encontrado." }));

        var entity = await _eventoService.CreateAsync(dto.ToEntity(userId), dto.ContatosParticipantes);
        return Ok(GenericResponse<ListarEventosDTO>.SucessoResponse(entity.ToRequest(), "Evento criado com sucesso."));
    }

    [HttpPost("{eventoId}/status/{status}")]
    [Authorize]
    public async Task<ActionResult<GenericResponse<CriarEventoDTO>>> AlterarStatusEvento(Guid eventoId, int status)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdClaim, out var userId))
            return Unauthorized(GenericResponse<string>.ErroResponse(new List<string> { "Usuário não encontrado." }));

        var evento = await _eventoService.GetByIdAsync(eventoId);
        if (evento == null)
            return NotFound(GenericResponse<string>.ErroResponse(new List<string> { "Evento não encontrado." }));

        var novoStatus = (StatusEvento)status;
        var erros = new List<string>();

        switch ((StatusEvento)evento.Status)
        {
            case StatusEvento.Aberto:
                if (novoStatus != StatusEvento.Fechado && novoStatus != StatusEvento.Cancelado)
                    erros.Add("Um evento ABERTO só pode ser alterado para FECHADO ou CANCELADO.");
                break;

            case StatusEvento.Fechado:
                if (novoStatus != StatusEvento.Finalizado && novoStatus != StatusEvento.Cancelado)
                    erros.Add("Um evento FECHADO só pode ser alterado para FINALIZADO ou CANCELADO.");
                break;

            case StatusEvento.Finalizado:
                erros.Add("Um evento FINALIZADO não pode ter seu status alterado.");
                break;

            case StatusEvento.Cancelado:
                erros.Add("Um evento CANCELADO não pode ter seu status alterado.");
                break;

            default:
                erros.Add("Status atual inválido.");
                break;
        }

        if (erros.Any())
            return BadRequest(GenericResponse<List<string>>.ErroResponse(erros, "Erro ao alterar status do evento."));

        await _eventoService.UpdateStatusAsync(evento, novoStatus);
        return Ok(GenericResponse<string>.SucessoResponse("", "Status do evento alterado com sucesso."));
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<GenericResponse<ListarEventosDTO>>> GetById(Guid id)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdClaim, out var userId))
            return Unauthorized(GenericResponse<string>.ErroResponse(new List<string> { "Usuário não encontrado." }));

        var evento = await _eventoService.GetByIdAsync(id);
        return Ok(
            GenericResponse<ListarEventosDTO>.SucessoResponse(evento.ToRequest(), "Evento carregado com sucesso."));
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> Update(Guid id, [FromBody] EditarEventoDTO dto)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdClaim, out var userId))
            return Unauthorized(GenericResponse<string>.ErroResponse(new List<string> { "Usuário não encontrado." }));

        await _eventoService.UpdateAsync(dto, id);
        return Ok(GenericResponse<string>.SucessoResponse("", "Evento atualizado com sucesso."));
    }
}