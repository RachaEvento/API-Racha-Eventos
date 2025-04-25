using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrganizadorEventos.Interfaces.Services;
using OrganizadorEventos.Model;
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

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAll()
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!Guid.TryParse(userIdClaim, out var userId))
            return Unauthorized(GenericResponse<string>.ErroResponse(new List<string> { "Usuário não encontrado." }));

        var contatos = await _contatoService.GetAllByUserAsync(userId.ToString());
        var contatoRequests = contatos.Select(c => new ContatoRequest(c)).ToList();

        return Ok(GenericResponse<IEnumerable<ContatoRequest>>.SucessoResponse(contatoRequests,
            "Contatos carregados com sucesso."));
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetById(Guid id)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!Guid.TryParse(userIdClaim, out var userId))
            return Unauthorized(GenericResponse<string>.ErroResponse(new List<string> { "Usuário não encontrado." }));

        var contato = await _contatoService.GetByIdAsync(id);
        if (contato == null || contato.UsuarioId != userId.ToString())
            return NotFound(GenericResponse<string>.ErroResponse(new List<string> { "Contato não encontrado." },
                "Erro ao buscar contato."));

        return Ok(GenericResponse<ContatoRequest>.SucessoResponse(new ContatoRequest(contato), "Contato encontrado."));
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] ContatoRequest contato)
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

        //passo apenas o request de contato e o id do usuário da requisição, o id do contato será criado um novo
        var created = await _contatoService.CreateAsync(new Contato(contato, userId.ToString()));
        return Ok(GenericResponse<ContatoRequest>.SucessoResponse(new ContatoRequest(created),
            "Contato criado com sucesso."));
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> Update(Guid id, [FromBody] ContatoRequest contato)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!Guid.TryParse(userIdClaim, out var userId))
            return Unauthorized(GenericResponse<string>.ErroResponse(new List<string> { "Usuário não encontrado." }));

        //Passa contato as infos de update, o id do usuario e id do contato a ser atualizado 
        await _contatoService.UpdateAsync(new Contato(contato, userId.ToString(), id));
        return Ok(GenericResponse<string>.SucessoResponse("Contato atualizado com sucesso."));
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _contatoService.DeleteAsync(id);
        return Ok(GenericResponse<string>.SucessoResponse("Contato excluído com sucesso."));
    }
}