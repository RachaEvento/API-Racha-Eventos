using OrganizadorEventos.DTOs.Custos;
using OrganizadorEventos.Interfaces.Repositories;
using OrganizadorEventos.Interfaces.Services;

namespace OrganizadorEventos.Services;

public class ParticipanteListaCustoService : IParticipanteListaCustoService
{
    private readonly IParticipanteListaCustoRepository _participanteListaCustoRepository;

    public ParticipanteListaCustoService(IParticipanteListaCustoRepository participanteListaCustoRepository)
    {
        _participanteListaCustoRepository = participanteListaCustoRepository;
    }

    public async Task AdicionarParticipantesAsync(AdicionarParticipantesListaCustoDTO dto)
    {
        await _participanteListaCustoRepository.CreateAllAsync(dto.ListaCustoId, dto.ParticipantesIds);
    }

    public async Task RemoverParticipantesAsync(RemoverParticipantesListaCustoDTO dto)
    {
        await _participanteListaCustoRepository.RemoveAllAsync(dto.ListaCustoId, dto.ParticipantesIds);
    }
}