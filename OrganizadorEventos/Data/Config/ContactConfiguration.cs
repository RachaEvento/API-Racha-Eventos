using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrganizadorEventos.Model;

namespace OrganizadorEventos.Data.Config;

public class ContactConfiguration : IEntityTypeConfiguration<Contact>
{
    public void Configure(EntityTypeBuilder<Contact> builder)
    {
        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(c => c.PhoneNumber)
            .IsRequired()
            .HasMaxLength(15);

        // Configura o relacionamento entre Contact e User
        builder.HasOne(c => c.User) // Um contato pertence a um usuário
            .WithMany(u => u.Contacts) // Um usuário pode ter muitos contatos
            .HasForeignKey(c => c.UserId) // A chave estrangeira que vai conectar com a tabela de 'User'
            .HasPrincipalKey(u => u.Id); // Relaciona com a chave primária do usuário
    }
}