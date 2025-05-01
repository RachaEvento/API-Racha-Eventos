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

    public async Task AdicionarParticipantesAsync(Guid listaCustoId, AdicionarParticipantesListaCustoDTO dto)
    {
        await _participanteListaCustoRepository.CreateAllAsync(listaCustoId, dto.ParticipantesIds);
    }

    public async Task RemoverParticipantesAsync(Guid listaCustoId, RemoverParticipantesListaCustoDTO dto)
    {
        await _participanteListaCustoRepository.RemoveAllAsync(listaCustoId, dto.ParticipantesIds);
    }
}