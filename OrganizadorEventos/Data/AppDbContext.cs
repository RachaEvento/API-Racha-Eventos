using Ardalis.EFCore.Extensions;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OrganizadorEventos.Model;

namespace OrganizadorEventos.Data;

public class AppDbContext : IdentityDbContext<User>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<EventParticipants> EventParticipants { get; set; }
    public DbSet<EventCost> EventCosts { get; set; }
    public DbSet<Contact> Contacts { get; set; }
    public DbSet<Event> Events { get; set; }
    public DbSet<Payment> Payments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyAllConfigurationsFromCurrentAssembly();
    }
}