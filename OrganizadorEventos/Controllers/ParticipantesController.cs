using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrganizadorEventos.DTOs.Email;
using OrganizadorEventos.DTOs.Participantes;
using OrganizadorEventos.Enum;
using OrganizadorEventos.Interfaces.Services;
using OrganizadorEventos.Request;
using OrganizadorEventos.Response;

namespace OrganizadorEventos.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ParticipantesController : ControllerBase
{
    private readonly IParticipanteService _participanteService;
    private readonly IGoogleDriveService _googleDriveService;

    public ParticipantesController(IParticipanteService participanteService, IGoogleDriveService googleDriveService)
    {
        _participanteService = participanteService;
        _googleDriveService = googleDriveService;
    }
    
    #region Endpoints de gerência do participante
    
    [HttpGet("evento/{EventoId}")]
    [Authorize]
    public async Task<IActionResult> ListParticipants(Guid EventoId)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdClaim, out var userId))
            return Unauthorized(GenericResponse<string>.ErroResponse(new List<string> { "Usuário não encontrado." }));

        var participantes = await _participanteService.ListarParticipantes(EventoId);
        return Ok(GenericResponse<List<ParticipanteDTO>>.SucessoResponse(participantes,
            "Participantes recuperados com sucesso."));
    }
    
    [HttpPost("evento/{EventoId}/adicionar")]
    [Authorize]
    public async Task<IActionResult> AddParticipants(Guid EventoId, [FromBody] AdicionarParticipanteDTO contatos)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdClaim, out var userId))
            return Unauthorized(GenericResponse<string>.ErroResponse(new List<string> { "Usuário não encontrado." }));
        
        await _participanteService.AdicionarContatosComoParticipantesAsync(contatos, EventoId);
        return Ok(GenericResponse<string>.SucessoResponse("","Participantes adicionados com sucesso."));
    }
    
    [HttpPost("evento/{EventoId}/remover")]
    [Authorize]
    public async Task<IActionResult> RemoveParticipants(Guid EventoId, [FromBody] RemoverParticipanteDTO participantes)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdClaim, out var userId))
            return Unauthorized(GenericResponse<string>.ErroResponse(new List<string> { "Usuário não encontrado." }));

        await _participanteService.RemoverParticipantesAsync(participantes, EventoId);
        return Ok(GenericResponse<string>.SucessoResponse("", "Participantes removidos com sucesso."));
    }
    
    [HttpPost("evento/{EventoId}/convidar/todos")]
    [Authorize]
    public async Task<IActionResult> ConvidarParticipantesEvento(Guid EventoId)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdClaim, out var userId))
            return Unauthorized(GenericResponse<string>.ErroResponse(new List<string> { "Usuário não encontrado." }));

        await _participanteService.ConvidarTodosParticipantesEvento(EventoId);
        return Ok(GenericResponse<string>.SucessoResponse("","Emails encaminhados"));
    }
    
    [HttpPost("evento/{EventoId}/convidar/{ParticipanteId}")]
    [Authorize]
    public async Task<IActionResult> ConvidarParticipanteEvento(Guid EventoId, Guid ParticipanteId)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdClaim, out var userId))
            return Unauthorized(GenericResponse<string>.ErroResponse(new List<string> { "Usuário não encontrado." }));

        await _participanteService.ConvidarParticipante(ParticipanteId);
        return Ok(GenericResponse<string>.SucessoResponse("","Email encaminhados"));
    }
    
    [HttpPost("{pagamentoId}/pagamento/aprovar")]
    [Authorize]
    public async Task<IActionResult> AprovarPagamentoParticipante(Guid pagamentoId)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdClaim, out var userId))
            return Unauthorized(GenericResponse<string>.ErroResponse(new List<string> { "Usuário não encontrado." }));

        await _participanteService.AlterarPagamentoParticipante(pagamentoId, StatusPagamento.Pago);
        return Ok(GenericResponse<string>.SucessoResponse("","Pagamento aprovado"));
    }
    
    [HttpPost("{pagamentoId}/pagamento/recusar")]
    [Authorize]
    public async Task<IActionResult> RecusarPagamentoParticipante(Guid pagamentoId)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdClaim, out var userId))
            return Unauthorized(GenericResponse<string>.ErroResponse(new List<string> { "Usuário não encontrado." }));

        await _participanteService.AlterarPagamentoParticipante(pagamentoId, StatusPagamento.Recusado);
        return Ok(GenericResponse<string>.SucessoResponse("","Pagamento recusado"));
    }
    
    [HttpGet("{ParticipanteId}/pagamento/listar")]
    public async Task<IActionResult> ListarPagamentosParticipante(Guid ParticipanteId)
    {
        var listaPagamentos = await _participanteService.ListarPagamentoParticipante(ParticipanteId);
        return Ok(GenericResponse<ListaPagamentosDTO>.SucessoResponse(listaPagamentos,"Pagamentos recuperado com sucesso."));
    }
    
    #endregion
    
    #region Sem Authorize (endpoints publicos de convite/pagamento)
    
    [HttpPost("{ParticipanteId}/confirmar")]
    public async Task<IActionResult> ConfirmarParticipante(Guid ParticipanteId)
    {
        await _participanteService.ConfirmarParticipante(ParticipanteId);
        return Ok(GenericResponse<string>.SucessoResponse("","Participante confirmado."));
    }
    
    [HttpPost("{ParticipanteId}/recusar")]
    public async Task<IActionResult> RecusarParticipante(Guid ParticipanteId)
    {
        await _participanteService.RecusarParticipante(ParticipanteId);
        return Ok(GenericResponse<string>.SucessoResponse("","Participante recusado."));
    }
    
    [HttpGet("{ParticipanteId}/convite")]
    public async Task<IActionResult> ConviteParticipante(Guid ParticipanteId)
    {
        var convite = await _participanteService.ConviteParticipante(ParticipanteId);
        return Ok(GenericResponse<ConviteParticipanteDTO>.SucessoResponse(convite,"Convite recuperado com sucesso."));
    }
    
    [HttpGet("{ParticipanteId}/pagamento")]
    public async Task<IActionResult> InformacoesPagamento(Guid ParticipanteId)
    {
        var informacoes = await _participanteService.InformacoesPagamento(ParticipanteId);
        return Ok(GenericResponse<InformacoesPagamentoDTO>.SucessoResponse(informacoes,"Informações recuperadas com sucesso."));
    }
    
    [HttpPost("{ParticipanteId}/pagamento")]
    public async Task<IActionResult> EnviarPagamento(Guid ParticipanteId, IFormFile file)
    {
        try
        {
            var fileId = await _googleDriveService.UploadFileAsync(file);
            await _participanteService.SalvarPagamento(ParticipanteId, fileId);

            return Ok(GenericResponse<string>.SucessoResponse("", "Pagamento enviado para avaliação."));
        }
        catch (Exception ex)
        {
            return BadRequest(GenericResponse<string>.ErroResponse(new List<string> {ex.InnerException?.Message ?? ex.Message}, "Erro ao enviar pagamento."));
        }
    }
    
    #endregion
}