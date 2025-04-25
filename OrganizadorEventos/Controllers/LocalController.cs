using Microsoft.AspNetCore.Mvc;
using OrganizadorEventos.Interfaces.Repositories;
using OrganizadorEventos.Mappers;
using OrganizadorEventos.Request;
using OrganizadorEventos.Response;

namespace OrganizadorEventos.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LocalController : ControllerBase
{
    private readonly ILocalRepository _localRepository;

    public LocalController(ILocalRepository localRepository)
    {
        _localRepository = localRepository;
    }

    [HttpGet("GetAll")]
    public async Task<ActionResult<GenericResponse<IEnumerable<LocalRequest>>>> GetAll()
    {
        var locais = await _localRepository.GetAllAsync();

        // Mapeando a lista de Local para LocalRequest usando o LocalMapper
        var localRequests = locais.Select(local => LocalMapper.ToLocalRequest(local)).ToList();

        var response = GenericResponse<IEnumerable<LocalRequest>>.SucessoResponse(localRequests);
        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GenericResponse<LocalRequest>>> GetById(Guid id)
    {
        var local = await _localRepository.GetByIdAsync(id);
        if (local == null)
            return NotFound("Local não encontrado.");

        // Mapeando de Local para LocalRequest
        var localRequest = LocalMapper.ToLocalRequest(local);

        var response = GenericResponse<LocalRequest>.SucessoResponse(localRequest);
        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<GenericResponse<LocalRequest>>> Create([FromBody] LocalRequest localRequest)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        // Mapeando de LocalRequest para Local (entidade)
        var local = LocalMapper.ToLocalEntity(localRequest);

        // Chama o repositório para salvar a entidade no banco
        var createdLocal = await _localRepository.CreateAsync(local);

        // Mapeando a entidade Local de volta para LocalRequest
        var localResponse = LocalMapper.ToLocalRequest(createdLocal);

        var response = GenericResponse<LocalRequest>.SucessoResponse(localResponse);
        return CreatedAtAction(nameof(GetById), new { id = createdLocal.LocalId }, response);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] LocalRequest localRequest)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var existingLocal = await _localRepository.GetByIdAsync(localRequest.LocalId);
        if (existingLocal == null)
            return NotFound("Local não encontrado.");

        LocalMapper.UpdateLocalEntity(existingLocal, localRequest);

        await _localRepository.UpdateAsync(existingLocal);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _localRepository.DeleteAsync(id);
        return NoContent();
    }
}