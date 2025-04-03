using Microsoft.EntityFrameworkCore;
using OrganizadorEventos.Data;
using OrganizadorEventos.Interfaces;
using OrganizadorEventos.Model;

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

    public async Task<Contact?> GetContactByIdAsync(Guid contactId)
    {
        return await _context.Contacts.FindAsync(contactId);
    }

    public async Task<List<Contact>> GetContactsByUserIdAsync(string userId)
    {
        return await _context.Contacts
            .Where(c => c.UserId == userId)
            .ToListAsync();
    }
}