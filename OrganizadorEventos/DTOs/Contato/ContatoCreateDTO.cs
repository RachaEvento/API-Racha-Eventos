namespace OrganizadorEventos.DTOs.Contato;

public class ContatoCreateDTO
{
    public string Nome { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Telefone { get; set; } = null!;
    public bool Ativo { get; set; }
    public IFormFile? Foto { get; set; }
}