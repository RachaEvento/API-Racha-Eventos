using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrganizadorEventos.DTOs.Participantes;
using OrganizadorEventos.Enum;
using OrganizadorEventos.Interfaces.Services;
using OrganizadorEventos.Response;

namespace OrganizadorEventos.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PagamentoController : ControllerBase
{
    private readonly IGoogleDriveService _googleDriveService;
    private readonly IPagamentoService _pagamentoService;

    public PagamentoController(IPagamentoService pagamentoService, IGoogleDriveService googleDriveService)
    {
        _pagamentoService = pagamentoService;
        _googleDriveService = googleDriveService;
    }

    #region Endpoints de gerência do organizador

    [HttpPost("{pagamentoId}/aprovar")]
    [Authorize]
    public async Task<IActionResult> AprovarPagamentoParticipante(Guid pagamentoId)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdClaim, out var userId))
            return Unauthorized(GenericResponse<string>.ErroResponse(new List<string> { "Usuário não encontrado." }));

        await _pagamentoService.AlterarPagamentoParticipante(pagamentoId, StatusPagamento.Pago);
        return Ok(GenericResponse<string>.SucessoResponse("", "Pagamento aprovado"));
    }

    [HttpPost("{pagamentoId}/recusar")]
    [Authorize]
    public async Task<IActionResult> RecusarPagamentoParticipante(Guid pagamentoId)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdClaim, out var userId))
            return Unauthorized(GenericResponse<string>.ErroResponse(new List<string> { "Usuário não encontrado." }));

        await _pagamentoService.AlterarPagamentoParticipante(pagamentoId, StatusPagamento.Recusado);
        return Ok(GenericResponse<string>.SucessoResponse("", "Pagamento recusado"));
    }

    [HttpGet("{ParticipanteId}/listar")]
    public async Task<IActionResult> ListarPagamentosParticipante(Guid ParticipanteId)
    {
        var listaPagamentos = await _pagamentoService.ListarPagamentoParticipante(ParticipanteId);
        return Ok(GenericResponse<ListaPagamentosDTO>.SucessoResponse(listaPagamentos,
            "Pagamentos recuperado com sucesso."));
    }

    #endregion

    #region Endpoints de acesso público

    [HttpGet("{ParticipanteId}")]
    public async Task<IActionResult> InformacoesPagamento(Guid ParticipanteId)
    {
        var informacoes = await _pagamentoService.InformacoesPagamento(ParticipanteId);
        return Ok(GenericResponse<InformacoesPagamentoDTO>.SucessoResponse(informacoes,
            "Informações recuperadas com sucesso."));
    }

    [HttpPost("{ParticipanteId}")]
    public async Task<IActionResult> EnviarPagamento(Guid ParticipanteId, IFormFile file)
    {
        try
        {
            //var fileId = await _googleDriveService.UploadFileAsync(file); Desabilitado porque estava causando problemas
            if (file == null || file.Length == 0) return BadRequest("Arquivo não enviado.");

            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            var fileBytes = memoryStream.ToArray();
            var base64String = Convert.ToBase64String(fileBytes);

            await _pagamentoService.SalvarPagamento(ParticipanteId, base64String);

            return Ok(GenericResponse<string>.SucessoResponse("", "Pagamento enviado para avaliação."));
        }
        catch (Exception ex)
        {
            return BadRequest(GenericResponse<string>.ErroResponse(
                new List<string> { ex.InnerException?.Message ?? ex.Message }, "Erro ao enviar pagamento."));
        }
    }

    #endregion
}