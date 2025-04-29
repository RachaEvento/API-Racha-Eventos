namespace OrganizadorEventos.Model;

public class Evento
{
    public Guid Id { get; set; }
    public Guid UsuarioId { get; set; }
    public Guid? LocalId { get; set; }
    public string Nome { get; set; } = null!;
    public string? Descricao { get; set; }
    public DateTime DataInicio { get; set; }
    public DateTime? DataFinal { get; set; }
    public int Status { get; set; }

    //Snapshot do local no momento (Denormalização)
    public string? LocalDescricaoLocal { get; set; }
    public string? LocalNome { get; set; }
    public string? LocalEndereco { get; set; }
    public string? LocalBairro { get; set; }
    public string? LocalCidade { get; set; }
    public string? LocalEstado { get; set; }

    public Usuario Usuario { get; set; } = null!;
    public Local? Local { get; set; }
    public ICollection<ListaCusto> ListaCustos { get; set; } = new List<ListaCusto>();
    public ICollection<Participante> Participantes { get; set; } = new List<Participante>();
}