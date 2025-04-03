using OrganizadorEventos.Model;

namespace OrganizadorEventos.Interfaces;

public interface ILocationRepository
{
    Task<Location?> GetLocationByIdAsync(Guid locationId);
}