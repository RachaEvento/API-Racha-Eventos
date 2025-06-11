using Microsoft.AspNetCore.Identity;
using OrganizadorEventos.Enum;

namespace OrganizadorEventos.Model;

public class Usuario : IdentityUser<Guid>
{
    public ICollection<Local> Locais { get; set; } = new List<Local>();
    public ICollection<Evento> Eventos { get; set; } = new List<Evento>();
    public ICollection<Contato> Contatos { get; set; } = new List<Contato>();
    public string? ChavePix { get; set; }
    public TipoChavePix? TipoChavePix { get; set; }
    public string Name { get; set; }

}