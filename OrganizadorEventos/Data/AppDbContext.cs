using Ardalis.EFCore.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OrganizadorEventos.Model;

namespace OrganizadorEventos.Data;

public class AppDbContext : IdentityDbContext<Usuario, IdentityRole<Guid>, Guid>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    
    public override int SaveChanges()
    {
        PreencherSnapshotLocal();
        return base.SaveChanges();
    }
    
    public override Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        PreencherSnapshotLocal();
        return base.SaveChangesAsync(ct);
    }
    private void PreencherSnapshotLocal()
    {
        var entries = ChangeTracker
            .Entries<Evento>()
            .Where(e => e.State is EntityState.Added or EntityState.Modified);

        foreach (var entry in entries)
        {
            var ev = entry.Entity;

            if (ev.Local is not null)
            {
                ev.LocalDescricaoLocal = ev.Local.DescricaoLocal;
                ev.LocalNome           = ev.Local.Nome;
                ev.LocalEndereco       = ev.Local.Endereco;
                ev.LocalBairro         = ev.Local.Bairro;
                ev.LocalCidade         = ev.Local.Cidade;
                ev.LocalEstado         = ev.Local.Estado;
            }
            else if (ev.LocalId.HasValue)
            {
                var local = Locais.Find(ev.LocalId.Value);
                if (local is not null)
                {
                    ev.LocalDescricaoLocal = local.DescricaoLocal;
                    ev.LocalNome           = local.Nome;
                    ev.LocalEndereco       = local.Endereco;
                    ev.LocalBairro         = local.Bairro;
                    ev.LocalCidade         = local.Cidade;
                    ev.LocalEstado         = local.Estado;
                }
            }
            else
            {
                ev.LocalDescricaoLocal = null;
                ev.LocalNome           = null;
                ev.LocalEndereco       = null;
                ev.LocalBairro         = null;
                ev.LocalCidade         = null;
                ev.LocalEstado         = null;
            }
        }
    }
    

    public DbSet<Local> Locais { get; set; }
    public DbSet<Evento> Eventos { get; set; }
    public DbSet<ListaCusto> ListaCustos { get; set; }
    public DbSet<Custo> Custos { get; set; }
    public DbSet<Contato> Contatos { get; set; }
    public DbSet<Participante> Participantes { get; set; }
    public DbSet<ParticipanteListaCusto> ParticipanteListaCustos { get; set; }
    public DbSet<PagamentoParticipante> PagamentoParticipantes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyAllConfigurationsFromCurrentAssembly();
    }
}