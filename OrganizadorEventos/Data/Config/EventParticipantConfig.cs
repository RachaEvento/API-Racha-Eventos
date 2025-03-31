using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrganizadorEventos.Model;

namespace OrganizadorEventos.Data.Config;

public class EventParticipantConfig : IEntityTypeConfiguration<EventParticipants>
{
    public void Configure(EntityTypeBuilder<EventParticipants> builder)
    {
        builder.ToTable("EventParticipants");

        builder.HasKey(ep => new { ep.EventId, ep.ContactId });

        // Relacionamento com a tabela Event (um evento pode ter vários participantes)
        builder.HasOne(ep => ep.Event)
            .WithMany(e => e.EventParticipants)
            .HasForeignKey(ep => ep.EventId)
            .OnDelete(DeleteBehavior.Cascade);

        // Relacionamento com a tabela Contact (um contato pode estar em vários eventos)
        builder.HasOne(ep => ep.Contact)
            .WithMany(c => c.EventParticipants)
            .HasForeignKey(ep => ep.ContactId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}