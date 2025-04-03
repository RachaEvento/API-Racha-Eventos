using OrganizadorEventos.Model;
using OrganizadorEventos.Request;

namespace OrganizadorEventos.Interfaces;

public interface IContactRepository
{
    Task<Contact> CreateContactAsync(Contact contact);
    Task<Contact?> GetContactByIdAsync(Guid contactId, string userId);
    Task<List<Contact>> GetContactsFiltroAsync(string userId, string? name, string? email, string? phone);
    Task<Contact?> UpdateContactAsync(Guid contactId, string userId, UpdateContactRequest updateRequest);
    Task<bool> DeactivateContactAsync(Guid contactId, string userId);
}