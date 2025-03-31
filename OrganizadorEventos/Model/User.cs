using Microsoft.AspNetCore.Identity;

namespace OrganizadorEventos.Model;

public class User : IdentityUser
{
    public string Nome { get; set; }
    public string Numero { get; set; }

    public ICollection<Contact> Contacts { get; set; } = new List<Contact>();

    public async Task<List<string>> GetRolesAsync(UserManager<User> userManager)
    {
        return (List<string>)await userManager.GetRolesAsync(this);
    }
}