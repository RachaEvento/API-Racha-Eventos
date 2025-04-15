using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrganizadorEventos.Model;

namespace OrganizadorEventos.Data.Config;

public class ParticipanteListaCustoConfiguration : IEntityTypeConfiguration<ParticipanteListaCusto>
{
    public void Configure(EntityTypeBuilder<ParticipanteListaCusto> builder)
    {
        builder.HasKey(plc => new { plc.ListaCustoId, plc.ParticipanteId });
    }
}