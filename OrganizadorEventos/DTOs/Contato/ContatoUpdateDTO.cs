
namespace OrganizadorEventos.DTOs.Contato
{
    public class ContatoUpdateDTO
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string? Email { get; set; }
        public string? Telefone { get; set; }
        public IFormFile? Foto { get; set; } 
    }
}