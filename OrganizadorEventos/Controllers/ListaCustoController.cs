using Microsoft.AspNetCore.Mvc;
using OrganizadorEventos.DTOs.Custos;
using OrganizadorEventos.Interfaces.Services;
using OrganizadorEventos.Request.Evento;
using OrganizadorEventos.Response;

namespace OrganizadorEventos.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ListaCustoController : ControllerBase
    {
        private readonly IListaCustoService _listaCustoService;
        private readonly IParticipanteListaCustoService _participanteListaCustoService;

        public ListaCustoController(
            IListaCustoService listaCustoService,
            IParticipanteListaCustoService participanteListaCustoService)
        {
            _listaCustoService = listaCustoService;
            _participanteListaCustoService = participanteListaCustoService;
        }

        [HttpPost]
        public async Task<ActionResult<GenericResponse<string>>> CriarListaCusto([FromBody] CriarListaCustoDTO dto)
        {
            var response = await _listaCustoService.CriarListaCustoAsync(dto);
            return Ok(response);
        }

        [HttpGet("evento/{eventoId}")]
        public async Task<ActionResult<GenericResponse<List<ListaCustoResponseDTO>>>> ListarListasPorEvento(Guid eventoId)
        {
            var listas = await _listaCustoService.ListarListasDeCustoPorEventoAsync(eventoId);
            return Ok(GenericResponse<List<ListaCustoResponseDTO>>.SucessoResponse(listas));
        }
        
        [HttpGet("evento/{eventoId}/custos")]
        public async Task<IActionResult> GetEventosComCustos(Guid eventoId)
        {
            try
            {
                var eventos = await _listaCustoService.ListarEventosComCustosAsync(eventoId);
                return Ok(GenericResponse<List<EventoComCustosDto>>.SucessoResponse(eventos));
            }
            catch (Exception ex)
            {
                return Ok(GenericResponse<string>.ErroResponse(new List<string>(), $"Erro ao obter eventos: {ex.Message}"));
            }
        }

        [HttpPost("participantes/adicionar")]
        public async Task<ActionResult<GenericResponse<string>>> AdicionarParticipantes(
            [FromBody] AdicionarParticipantesListaCustoDTO dto)
        {
            await _participanteListaCustoService.AdicionarParticipantesAsync(dto);
            return Ok(GenericResponse<string>.SucessoResponse(null, "Participantes adicionados com sucesso."));
        }

        [HttpPost("participantes/remover")]
        public async Task<ActionResult<GenericResponse<string>>> RemoverParticipantes(
            [FromBody] RemoverParticipantesListaCustoDTO dto)
        {
            await _participanteListaCustoService.RemoverParticipantesAsync(dto);
            return Ok(GenericResponse<string>.SucessoResponse(null, "Participantes removidos com sucesso."));
        }
    }
}
