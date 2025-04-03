using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrganizadorEventos.Interfaces;
using OrganizadorEventos.Model;
using OrganizadorEventos.Request;

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
            return BadRequest("Nome e telefone são obrigatórios.");

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
            return Unauthorized("Usuário não autenticado.");

        var contact = new Contact
        {
            Name = contactRequest.Name,
            Email = contactRequest.Email,
            PhoneNumber = contactRequest.PhoneNumber,
            UserId = userId,
            IsActive = true
        };

        var newContact = await _contactRepository.CreateContactAsync(contact);
        return CreatedAtAction(nameof(GetContactById), new { contactId = newContact.Id }, newContact);
    }

    [HttpGet("FiltroUsuarios")]
    [Authorize]
    public async Task<IActionResult> GetContacts([FromQuery] string? name, [FromQuery] string? email,
        [FromQuery] string? phone)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
            return Unauthorized("Usuário não autenticado.");

        var contacts = await _contactRepository.GetContactsFiltroAsync(userId, name, email, phone);
        return Ok(contacts);
    }

    [HttpGet("get/{contactId}")]
    [Authorize]
    public async Task<IActionResult> GetContactById(Guid contactId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
            return Unauthorized("Usuário não autenticado.");

        var contact = await _contactRepository.GetContactByIdAsync(contactId, userId);
        if (contact == null)
            return NotFound("Contato não encontrado ou não autorizado.");

        return Ok(contact);
    }

    [HttpPut("update/{contactId}")]
    [Authorize]
    public async Task<IActionResult> UpdateContact(Guid contactId, [FromBody] UpdateContactRequest updateRequest)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
            return Unauthorized("Usuário não autenticado.");

        var updatedContact = await _contactRepository.UpdateContactAsync(contactId, userId, updateRequest);
        if (updatedContact == null)
            return NotFound("Contato não encontrado ou não autorizado.");

        return Ok(updatedContact);
    }

    [HttpDelete("delete/{contactId}")]
    [Authorize]
    public async Task<IActionResult> DeleteContact(Guid contactId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
            return Unauthorized("Usuário não autenticado.");

        var result = await _contactRepository.DeactivateContactAsync(contactId, userId);
        if (!result)
            return NotFound("Contato não encontrado ou não autorizado.");

        return NoContent();
    }
}