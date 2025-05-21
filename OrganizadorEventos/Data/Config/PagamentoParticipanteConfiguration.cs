using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrganizadorEventos.Model;

namespace OrganizadorEventos.Data.Config;

public class PagamentoParticipanteConfiguration : IEntityTypeConfiguration<PagamentoParticipante>
{
    public void Configure(EntityTypeBuilder<PagamentoParticipante> builder)
    {
        builder.Property(p => p.DataPagamento)
            .HasDefaultValueSql("NOW()");
    }
}