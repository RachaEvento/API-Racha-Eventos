using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrganizadorEventos.Interfaces.Services;
using OrganizadorEventos.Mappers;
using OrganizadorEventos.Request.Local;
using OrganizadorEventos.Response;

namespace OrganizadorEventos.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LocaisController : ControllerBase
{
    private readonly ILocalService _localService;

    public LocaisController(ILocalService localService)
    {
        _localService = localService;
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<GenericResponse<IEnumerable<LocalRequest>>>> GetAll()
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!Guid.TryParse(userIdClaim, out var userId))
            return Unauthorized(GenericResponse<string>.ErroResponse(new List<string> { "Usuário não encontrado." }));

        var locais = await _localService.GetAllByUserAsync(userId);
        var locaisRequests = locais.Select(l => l.ToRequest()).ToList();

        return Ok(GenericResponse<IEnumerable<LocalRequest>>.SucessoResponse(locaisRequests,
            "Locais carregados com sucesso."));
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetById(Guid id)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!Guid.TryParse(userIdClaim, out var userId))
            return Unauthorized(GenericResponse<string>.ErroResponse(new List<string> { "Usuário não encontrado." }));

        var local = await _localService.GetByIdAsync(id);
        if (local == null || local.UsuarioId != userId)
            return NotFound(GenericResponse<string>.ErroResponse(new List<string> { "Local não encontrado." },
                "Erro ao buscar local."));

        return Ok(GenericResponse<LocalRequest>.SucessoResponse(local.ToRequest(), "Local encontrado."));
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<GenericResponse<LocalRequest>>> Create([FromBody] LocalRequest localRequest)
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

        //Cria um novo GUID antes de transformar em entity para garantir que o id seja único.
        localRequest.Id = Guid.NewGuid();

        var localCriado = await _localService.CreateAsync(localRequest.ToEntity(userId));
        return Ok(GenericResponse<LocalRequest>.SucessoResponse(localCriado.ToRequest(), "Local criado com sucesso."));
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> Update(Guid id, [FromBody] LocalRequest localRequest)
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

        localRequest.Id = id;
        await _localService.UpdateAsync(localRequest.ToEntity(userId));
        return Ok(GenericResponse<string>.SucessoResponse("", "Local atualizado com sucesso."));
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _localService.DeleteAsync(id);
        return Ok(GenericResponse<string>.SucessoResponse("", "Local excluído com sucesso."));
    }
}