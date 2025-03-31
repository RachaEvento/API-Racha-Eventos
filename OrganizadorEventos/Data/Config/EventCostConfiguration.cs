using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrganizadorEventos.Model;

namespace OrganizadorEventos.Data.Config;

public class EventCostConfiguration : IEntityTypeConfiguration<EventCost>
{
    public void Configure(EntityTypeBuilder<EventCost> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Description)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(c => c.Amount)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        // Relacionamento com a entidade Event (um evento pode ter múltiplos custos)
        builder.HasOne(c => c.Event)
            .WithMany(e => e.EventCosts)
            .HasForeignKey(c => c.EventId);
    }
}