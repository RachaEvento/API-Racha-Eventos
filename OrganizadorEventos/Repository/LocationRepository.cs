using Microsoft.EntityFrameworkCore;
using OrganizadorEventos.Data;
using OrganizadorEventos.Interfaces;
using OrganizadorEventos.Model;

namespace OrganizadorEventos.Repository;

public class LocationRepository : ILocationRepository
{
    private readonly AppDbContext _context;

    public LocationRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Location?> GetLocationByIdAsync(Guid locationId)
    {
        return await _context.Locations
            .FirstOrDefaultAsync(l => l.Id == locationId);
    }
}