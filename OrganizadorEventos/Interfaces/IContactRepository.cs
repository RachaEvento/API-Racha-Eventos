using OrganizadorEventos.Model;

namespace OrganizadorEventos.Interfaces;

public interface IContactRepository
{
    Task<Contact> CreateContactAsync(Contact contact);
    Task<Contact?> GetContactByIdAsync(Guid contactId);
    Task<List<Contact>> GetContactsByUserIdAsync(string userId);
}