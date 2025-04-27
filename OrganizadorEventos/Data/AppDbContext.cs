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