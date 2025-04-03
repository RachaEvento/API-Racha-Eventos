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
            PhoneNumber = contactRequest.PhoneNumber,
            UserId = userId
        };

        var newContact = await _contactRepository.CreateContactAsync(contact);

        return CreatedAtAction(nameof(GetContactById), new { contactId = newContact.Id }, newContact);
    }


    [HttpGet("get/{contactId}")]
    public async Task<IActionResult> GetContactById(Guid contactId)
    {
        var contact = await _contactRepository.GetContactByIdAsync(contactId);
        if (contact == null)
            return NotFound("Contato não encontrado.");

        return Ok(contact);
    }
}