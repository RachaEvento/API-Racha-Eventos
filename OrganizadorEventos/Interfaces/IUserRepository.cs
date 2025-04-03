using OrganizadorEventos.Model;

namespace OrganizadorEventos.Interfaces;

public interface IUserRepository
{
    Task<User> GetUserByIdAsync(string userId);
}