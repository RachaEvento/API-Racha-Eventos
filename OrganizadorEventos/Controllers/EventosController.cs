using Microsoft.AspNetCore.Mvc;
using OrganizadorEventos.Interfaces.Services;

namespace OrganizadorEventos.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EventosController : ControllerBase
{
    private readonly IEventoService _eventoService;

    public EventosController(IEventoService eventoService)
    {
        _eventoService = eventoService;
    }
}