using OrganizadorEventos.Model;
using OrganizadorEventos.Request;

namespace OrganizadorEventos.Interfaces.Services;

public interface IEventoService : ICrudService<Evento>
{
    Task<Evento> CreateEventAsync(CreateEventRequest request, string userId);
}