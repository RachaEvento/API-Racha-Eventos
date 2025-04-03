using Microsoft.EntityFrameworkCore;
using OrganizadorEventos.Data;
using OrganizadorEventos.Interfaces;
using OrganizadorEventos.Model;
using OrganizadorEventos.Request;

namespace OrganizadorEventos.Repository;

public class ContactRepository : IContactRepository
{
    private readonly AppDbContext _context;

    public ContactRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Contact> CreateContactAsync(Contact contact)
    {
        _context.Contacts.Add(contact);
        await _context.SaveChangesAsync();
        return contact;
    }

    public async Task<Contact?> GetContactByIdAsync(Guid contactId, string userId)
    {
        return await _context.Contacts.FirstOrDefaultAsync(c => c.Id == contactId && c.UserId == userId && c.IsActive);
    }

    public async Task<List<Contact>> GetContactsFiltroAsync(string userId, string? name, string? email, string? phone)
    {
        var query = _context.Contacts.Where(c => c.UserId == userId && c.IsActive);

        if (!string.IsNullOrEmpty(name))
            query = query.Where(c => c.Name.ToLower().Contains(name.ToLower()));

        if (!string.IsNullOrEmpty(email))
            query = query.Where(c => c.Email.Contains(email));

        if (!string.IsNullOrEmpty(phone))
            query = query.Where(c => c.PhoneNumber.Contains(phone));

        return await query.ToListAsync();
    }

    public async Task<Contact?> UpdateContactAsync(Guid contactId, string userId, UpdateContactRequest updateRequest)
    {
        var contact =
            await _context.Contacts.FirstOrDefaultAsync(c => c.Id == contactId && c.UserId == userId && c.IsActive);
        if (contact == null)
            return null;

        contact.Name = updateRequest.Name ?? contact.Name;
        contact.Email = updateRequest.Email ?? contact.Email;
        contact.PhoneNumber = updateRequest.PhoneNumber ?? contact.PhoneNumber;

        await _context.SaveChangesAsync();
        return contact;
    }

    public async Task<bool> DeactivateContactAsync(Guid contactId, string userId)
    {
        var contact =
            await _context.Contacts.FirstOrDefaultAsync(c => c.Id == contactId && c.UserId == userId && c.IsActive);
        if (contact == null)
            return false;

        contact.IsActive = false;
        await _context.SaveChangesAsync();
        return true;
    }
}