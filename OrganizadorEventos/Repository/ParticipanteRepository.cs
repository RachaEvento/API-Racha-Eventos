using Microsoft.EntityFrameworkCore;
using OrganizadorEventos.Data;
using OrganizadorEventos.Enum;
using OrganizadorEventos.Interfaces.Repositories;
using OrganizadorEventos.Model;

namespace OrganizadorEventos.Repository;

public class ParticipanteRepository : CrudRepository<Participante>, IParticipanteRepository
{
    private readonly AppDbContext _context;
    protected readonly DbSet<Participante> _dbSet;

    public ParticipanteRepository(AppDbContext context) : base(context)
    {
        _context = context;
        _dbSet = context.Set<Participante>();
    }

    public async Task<List<Participante>> CreateAllFromContactsAsync(List<Guid> contatosParticipantes, Guid eventoId)
    {
        var participantes = new List<Participante>();

        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            foreach (var contatoId in contatosParticipantes)
            {
                var participante = new Participante
                {
                    Id = Guid.NewGuid(),
                    EventoId = eventoId,
                    ContatoId = contatoId,
                    Status = (int)StatusParticipante.Pendente
                };
                participantes.Add(participante);
            }

            await _dbSet.AddRangeAsync(participantes);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
            return participantes;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw new Exception("Ocorreu um erro ao criar os participantes.", ex);
        }
    }

    public async Task RemoveParticipantsAsync(List<Guid> contatosParticipantes, Guid eventoId)
    {
        var participantes = await _dbSet
            .Where(p => p.EventoId == eventoId && contatosParticipantes.Contains(p.Id))
            .Include(p => p.Contato) // Eager load Contato
            .ToListAsync();

        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            if (participantes.Any())
            {
                var participanteIds = participantes.Select(p => p.Id).ToList();

                var participanteListaCustos = await _context.Set<ParticipanteListaCusto>()
                    .Where(plc => participanteIds.Contains(plc.ParticipanteId))
                    .ToListAsync();

                //Ideal seria converter isso posteriormente no repo de lista de custo
                _context.Set<ParticipanteListaCusto>().RemoveRange(participanteListaCustos);

                _dbSet.RemoveRange(participantes);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw new Exception("Ocorreu um erro ao remover os participantes do evento.", ex);
        }
    }

    public async Task<List<Participante>> GetAllByEventId(Guid eventoId)
    {
        return await _dbSet
            .Where(p => p.EventoId == eventoId)
            .Include(p => p.Contato) // Eager load Contato
            .Include(p => p.Pagamento)
            .ToListAsync();
    }

    public async Task<Participante> GetByIdAsync(Guid id)
    {
        return await _dbSet
            .Include(p => p.Contato)
            .ThenInclude(c => c.Usuario)
            .Include(p => p.Evento)
            .Include(p => p.Pagamento)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<List<Participante>> GetAllConfirmedByEventIdAsync(Guid eventoId)
    {
        return await _dbSet
            .Where(p => p.EventoId == eventoId && p.Status == (int)StatusParticipante.Confirmado)
            .Include(p => p.Contato) // Eager load Contato
            .ToListAsync();
    }

    public async Task UpdateAllNonConfirmedToDeniedByEventAsync(Guid eventoId)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var participantes = await _dbSet
                .Where(p => p.EventoId == eventoId && p.Status != (int)StatusParticipante.Confirmado)
                .ToListAsync();

            if (participantes.Any())
            {
                foreach (var participante in participantes)
                {
                    participante.Status = (int)StatusParticipante.Recusado;
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw new Exception("Ocorreu um erro ao atualizar os participantes para recusado.", ex);
        }
    }
}