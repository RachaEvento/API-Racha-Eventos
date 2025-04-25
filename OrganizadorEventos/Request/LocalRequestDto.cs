using System.ComponentModel.DataAnnotations;

namespace OrganizadorEventos.Request;

public class LocalRequest
{
    public Guid LocalId { get; set; }

    [Required] public string Nome { get; set; }

    [Required] public string Endereco { get; set; }

    [Required] public string DescricaoLocal { get; set; }

    [Required] public string Bairro { get; set; }

    [Required] public string Cidade { get; set; }

    [Required]
    [StringLength(2, MinimumLength = 2, ErrorMessage = "Estado deve ter 2 letras.")]
    public string Estado { get; set; }

    public bool Ativo { get; set; } = true;

    [Required] public string UsuarioId { get; set; }
}