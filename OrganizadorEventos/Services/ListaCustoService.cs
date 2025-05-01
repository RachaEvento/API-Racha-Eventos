using OrganizadorEventos.DTOs.Custos;
using OrganizadorEventos.DTOs.ListaCusto;
using OrganizadorEventos.Interfaces.Repositories;
using OrganizadorEventos.Interfaces.Services;
using OrganizadorEventos.Mappers;
using OrganizadorEventos.Model;
using OrganizadorEventos.Request;
using OrganizadorEventos.Request.Evento;
using OrganizadorEventos.Response;

namespace OrganizadorEventos.Services;

public class ListaCustoService : IListaCustoService
{
    private readonly IListaCustoRepository _listaCustoRepository;


    public ListaCustoService(
        IListaCustoRepository listaCustoRepository
    )
    {
        _listaCustoRepository = listaCustoRepository;
    }

    public async Task<string> CriarListaCustoAsync(Guid eventoId, CriarListaCustoDTO dto)
    {
        var listaCusto = new ListaCusto
        {
            Id = Guid.NewGuid(),
            EventoId = eventoId,
            Nome = dto.Nome
        };

        await _listaCustoRepository.CreateAsync(listaCusto);

        return "Lista de custo criada com sucesso.";
    }
    public async Task<List<ListaCustosComCustosEParticipantesDto>> ListarListaCustosComCustosEParticipantesAsync(Guid eventoId)
    {
        var eventos = await _listaCustoRepository.ObterTodosComCustosAsync(eventoId);
        
        return eventos.Select(lc => new ListaCustosComCustosEParticipantesDto
        {
            Id = lc.Id,
            Nome = lc.Nome,
            Custos = lc.Custos.Select(c => new CustoDTO
            {
                Id = c.Id,
                Nome = c.Nome,
                Valor = c.Valor
            }).ToList(),
            Participantes = lc.ParticipanteListaCustos.Select(c => new ContatoDTO
            {
                Id = c.Participante.Id,
                Nome = c.Participante.Contato.Nome,
                Email = c.Participante.Contato.Email,
                Telefone = c.Participante.Contato.Telefone,
                Ativo = c.Participante.Contato.Ativo
            }).ToList()
        }).ToList();
    }

}