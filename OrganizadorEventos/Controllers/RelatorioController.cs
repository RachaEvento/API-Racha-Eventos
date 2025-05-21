using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrganizadorEventos.DTOs;
using OrganizadorEventos.DTOs.Relatorio;
using OrganizadorEventos.Interfaces.Services;
using OrganizadorEventos.Response;

namespace OrganizadorEventos.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RelatorioController : ControllerBase
{
    private readonly IRelatorioEventoService _relatorioEventoService;
    private readonly IPixService _pixService;
    public RelatorioController(IRelatorioEventoService relatorioEventoService, IPixService pixService)
    {
        _relatorioEventoService = relatorioEventoService;
        _pixService = pixService;
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
    
    [HttpPost("generate-pix")]
    public async Task<IActionResult> GetPixQRCode([FromBody] PixRequest request)
    {
        try
        {
            string payload = await _pixService.GeneratePixQrCodeAsync(
                request.PixKey,
                request.ReceiverName,
                request.City,
                request.Amount,
                request.Message,
                request.TransactionId
            );
            
            return Ok(new { qrcodestring = "https://dyn-qrcode.vercel.app/api?url="+payload });
        }
        catch (Exception ex)
        {
            Debug.Write(ex.Message);
            return Ok(new { qrCodeBase64 = string.Empty });
        } 
    }
}