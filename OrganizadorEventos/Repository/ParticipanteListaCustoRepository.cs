using Microsoft.EntityFrameworkCore;
using OrganizadorEventos.Data;
using OrganizadorEventos.Interfaces.Repositories;
using OrganizadorEventos.Model;

namespace OrganizadorEventos.Repository;

public class ParticipanteListaCustoRepository : IParticipanteListaCustoRepository
{
    private readonly AppDbContext _context;

    public ParticipanteListaCustoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task CreateAllAsync(Guid listaCustoId, List<Guid> participanteIds)
    {
        var participantes = participanteIds.Select(id => new ParticipanteListaCusto
        {
            ListaCustoId = listaCustoId,
            ParticipanteId = id
        }).ToList();

        _context.ParticipanteListaCustos.AddRange(participantes);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveAllAsync(Guid listaCustoId, List<Guid> participanteIds)
    {
        var participantes = await _context.ParticipanteListaCustos
            .Where(p => p.ListaCustoId == listaCustoId && participanteIds.Contains(p.ParticipanteId))
            .ToListAsync();

        _context.ParticipanteListaCustos.RemoveRange(participantes);
        await _context.SaveChangesAsync();
    }
}