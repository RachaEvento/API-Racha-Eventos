using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrganizadorEventos.Model;

namespace OrganizadorEventos.Data.Config;

public class EventConfiguration : IEntityTypeConfiguration<Event>
{
    public void Configure(EntityTypeBuilder<Event> builder)
    {
        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(e => e.Description)
            .HasMaxLength(500);

        builder.Property(e => e.TotalPrice)
            .HasColumnType("decimal(18,2)");

        // Um evento para muitos custos
        builder.HasMany(e => e.EventCosts)
            .WithOne()
            .HasForeignKey(c => c.EventId);
    }
}