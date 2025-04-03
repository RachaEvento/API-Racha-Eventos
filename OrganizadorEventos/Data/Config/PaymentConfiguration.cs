using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrganizadorEventos.Model;

namespace OrganizadorEventos.Data.Config;

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.Property(p => p.Amount)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(p => p.PaymentDate)
            .IsRequired();

        // Múltiplos pagamentos a um evento
        builder.HasOne(p => p.Event)
            .WithMany(e => e.Payments)
            .HasForeignKey(p => p.EventId);
    }
}