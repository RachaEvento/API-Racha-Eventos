using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrganizadorEventos.Interfaces;
using OrganizadorEventos.Model;
using OrganizadorEventos.Request;
using OrganizadorEventos.Response;

namespace OrganizadorEventos.Controllers;

[Route("api/contacts")]
[ApiController]
public class ContactController : ControllerBase
{
    private readonly IContactRepository _contactRepository;

    public ContactController(IContactRepository contactRepository)
    {
        _contactRepository = contactRepository;
    }

    [HttpPost("createContact")]
    [Authorize]
    public async Task<IActionResult> CreateContact([FromBody] CreateContactRequest contactRequest)
    {
        if (string.IsNullOrWhiteSpace(contactRequest.Name) || string.IsNullOrWhiteSpace(contactRequest.PhoneNumber))
            return BadRequest(GenericResponse<string>.ErroResponse(new List<string>
                { "Nome e telefone são obrigatórios." }));

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
            return Unauthorized(GenericResponse<string>.ErroResponse(new List<string> { "Usuário não autenticado." }));

        var contact = new Contact
        {
            Name = contactRequest.Name,
            Email = contactRequest.Email,
            PhoneNumber = contactRequest.PhoneNumber,
            UserId = userId,
            IsActive = true
        };

        var newContact = await _contactRepository.CreateContactAsync(contact);
        return CreatedAtAction(nameof(GetContactById), new { contactId = newContact.Id },
            GenericResponse<Contact>.SucessoResponse(newContact, "Contato criado com sucesso."));
    }

    [HttpGet("FiltroUsuarios")]
    [Authorize]
    public async Task<IActionResult> GetContacts([FromQuery] string? name, [FromQuery] string? email,
        [FromQuery] string? phone)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
            return Unauthorized(GenericResponse<string>.ErroResponse(new List<string> { "Usuário não autenticado." }));

        var contacts = await _contactRepository.GetContactsFiltroAsync(userId, name, email, phone);
        return Ok(GenericResponse<IEnumerable<Contact>>.SucessoResponse(contacts, "Contatos encontrados."));
    }

    [HttpGet("get/{contactId}")]
    [Authorize]
    public async Task<IActionResult> GetContactById(Guid contactId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
            return Unauthorized(GenericResponse<string>.ErroResponse(new List<string> { "Usuário não autenticado." }));

        var contact = await _contactRepository.GetContactByIdAsync(contactId, userId);
        if (contact == null)
            return NotFound(GenericResponse<string>.ErroResponse(new List<string>
                { "Contato não encontrado ou não autorizado." }));

        return Ok(GenericResponse<Contact>.SucessoResponse(contact, "Contato encontrado."));
    }

    [HttpPut("update/{contactId}")]
    [Authorize]
    public async Task<IActionResult> UpdateContact(Guid contactId, [FromBody] UpdateContactRequest updateRequest)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
            return Unauthorized(GenericResponse<string>.ErroResponse(new List<string> { "Usuário não autenticado." }));

        var updatedContact = await _contactRepository.UpdateContactAsync(contactId, userId, updateRequest);
        if (updatedContact == null)
            return NotFound(GenericResponse<string>.ErroResponse(new List<string>
                { "Contato não encontrado ou não autorizado." }));

        return Ok(GenericResponse<Contact>.SucessoResponse(updatedContact, "Contato atualizado com sucesso."));
    }

    [HttpDelete("delete/{contactId}")]
    [Authorize]
    public async Task<IActionResult> DeleteContact(Guid contactId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
            return Unauthorized(GenericResponse<string>.ErroResponse(new List<string> { "Usuário não autenticado." }));

        var result = await _contactRepository.DeactivateContactAsync(contactId, userId);
        if (!result)
            return NotFound(GenericResponse<string>.ErroResponse(new List<string>
                { "Contato não encontrado ou não autorizado." }));

        return Ok(GenericResponse<string>.SucessoResponse(null, "Contato desativado com sucesso."));
    }
}