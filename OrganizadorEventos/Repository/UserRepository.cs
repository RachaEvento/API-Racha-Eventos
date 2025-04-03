using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OrganizadorEventos.Data;
using OrganizadorEventos.Interfaces;
using OrganizadorEventos.Model;

namespace OrganizadorEventos.Repository;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _dbContext;
    private readonly PasswordHasher<User> _passwordHasher;

    public UserRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
        _passwordHasher = new PasswordHasher<User>();
    }

    public async Task<User> GetUserByIdAsync(string userId)
    {
        return await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Id == userId);
    }
}