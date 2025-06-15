using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrganizadorEventos.DTOs.Contato;
using OrganizadorEventos.Interfaces.Services;
using OrganizadorEventos.Mappers;
using OrganizadorEventos.Request;
using OrganizadorEventos.Response;

namespace OrganizadorEventos.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ContatosController : ControllerBase
{
    private readonly IContatoService _contatoService;

    public ContatosController(IContatoService contatoService)
    {
        _contatoService = contatoService;
    }

    [HttpGet("disponiveis/{eventoId}")]
    [Authorize]
    public async Task<ActionResult<GenericResponse<IEnumerable<ContatoDTO>>>> GetAllByEvento(Guid eventoId)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!Guid.TryParse(userIdClaim, out var userId))
            return Unauthorized(GenericResponse<string>.ErroResponse(new List<string> { "Usuário não encontrado." }));

        var contatos = await _contatoService.GetAllByUserAndEventoAsync(userId, eventoId);
        var contatoRequests = contatos.Select(c => c.ToRequest()).ToList();

        return Ok(GenericResponse<IEnumerable<ContatoDTO>>.SucessoResponse(contatoRequests,
            "Contatos carregados com sucesso."));
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<GenericResponse<IEnumerable<ContatoDTO>>>> GetAll()
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!Guid.TryParse(userIdClaim, out var userId))
            return Unauthorized(GenericResponse<string>.ErroResponse(new List<string> { "Usuário não encontrado." }));

        var contatos = await _contatoService.GetAllByUserAsync(userId);
        var contatoRequests = contatos.Select(c => c.ToRequest()).ToList();

        return Ok(GenericResponse<IEnumerable<ContatoDTO>>.SucessoResponse(contatoRequests,
            "Contatos carregados com sucesso."));
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<GenericResponse<ContatoDTO>>> GetById(Guid id)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!Guid.TryParse(userIdClaim, out var userId))
            return Unauthorized(GenericResponse<string>.ErroResponse(new List<string> { "Usuário não encontrado." }));

        var contato = await _contatoService.GetByIdAsync(id);
        if (contato == null || contato.UsuarioId != userId)
            return NotFound(GenericResponse<string>.ErroResponse(new List<string> { "Contato não encontrado." },
                "Erro ao buscar contato."));

        return Ok(GenericResponse<ContatoDTO>.SucessoResponse(contato.ToRequest(), "Contato encontrado."));
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<GenericResponse<ContatoDTO>>> Create([FromForm] ContatoCreateDTO contatoDto)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdClaim, out var userId))
            return Unauthorized(GenericResponse<string>.ErroResponse(new List<string> { "Usuário não encontrado." }));

        byte[] fotoBytes = null;
        if (contatoDto.Foto != null && contatoDto.Foto.Length > 0)
        {
            using var ms = new MemoryStream();
            await contatoDto.Foto.CopyToAsync(ms);
            fotoBytes = ms.ToArray();
        }

        var contato = contatoDto.ToEntity(userId, fotoBytes);

        var contatoCriado = await _contatoService.CreateAsync(contato);

        return Ok(GenericResponse<ContatoDTO>.SucessoResponse(contatoCriado.ToRequest(), "Contato criado com sucesso."));
    }
    
    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> Update(Guid id, [FromForm] ContatoUpdateDTO contatoDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(GenericResponse<string>.ErroResponse(
                    ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList(),
                    "Erro de validação."
                )
            );

        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!Guid.TryParse(userIdClaim, out var userId))
            return Unauthorized(GenericResponse<string>.ErroResponse(new List<string> { "Usuário não encontrado." }));

        byte[] fotoBytes = null;
        if (contatoDto.Foto != null && contatoDto.Foto.Length > 0)
        {
            using var ms = new MemoryStream();
            await contatoDto.Foto.CopyToAsync(ms);
            fotoBytes = ms.ToArray();
        }

        var contato = contatoDto.ToEntity(userId, fotoBytes); 

        await _contatoService.UpdateAsync(contato);
        return Ok(GenericResponse<string>.SucessoResponse("", "Contato atualizado com sucesso."));
    }


    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _contatoService.DeleteAsync(id);
        return Ok(GenericResponse<string>.SucessoResponse("", "Contato excluído com sucesso."));
    }
}