using Microsoft.AspNetCore.Identity;

namespace OrganizadorEventos.Model;

public class Usuario : IdentityUser
{
    public ICollection<Local> Locais { get; set; } = new List<Local>();
    public ICollection<Evento> Eventos { get; set; } = new List<Evento>();
    public ICollection<Contato> Contatos { get; set; } = new List<Contato>();
}