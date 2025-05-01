using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrganizadorEventos.DTOs.Relatorio;
using OrganizadorEventos.Interfaces.Services;
using OrganizadorEventos.Response;

namespace OrganizadorEventos.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RelatorioController : ControllerBase
{
    private readonly IRelatorioEventoService _relatorioEventoService;
    public RelatorioController(IRelatorioEventoService relatorioEventoService)
    {
        _relatorioEventoService = relatorioEventoService;
    }

    [HttpGet("{eventoId}")]
    [Authorize]
    public async Task<IActionResult> GetDivisaoCustosEvento(Guid eventoId)
    {
        try
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!Guid.TryParse(userIdClaim, out var userId))
                return Unauthorized(
                    GenericResponse<string>.ErroResponse(new List<string> { "Usuário não encontrado." }));

            var custosParticipantes = await _relatorioEventoService.CalcularCustoParticipantesAsync(eventoId);
            return Ok(GenericResponse<List<CustoParticipanteDTO>>.SucessoResponse(custosParticipantes,
                "Custos dos participantes calculados."));
        }
        catch (Exception ex)
        {
            return NotFound(GenericResponse<string>.ErroResponse(new List<string> {ex.Message},"Erro ao calcular custos."));
        } 
    }
}