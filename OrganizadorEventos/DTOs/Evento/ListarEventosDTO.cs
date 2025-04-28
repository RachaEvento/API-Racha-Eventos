namespace OrganizadorEventos.Request.Evento;

public class ListarEventosDTO
{
    public Guid?    Id                   { get; set; }
    public Guid?   LocalId              { get; set; }

    public string  Nome                 { get; set; } = null!;
    public string? Descricao            { get; set; }

    public DateTime DataInicio          { get; set; }
    public DateTime? DataFinal          { get; set; }
    public int     Status               { get; set; }

    // snapshot do local
    public string? LocalDescricaoLocal  { get; set; }
    public string? LocalNome            { get; set; }
    public string? LocalEndereco        { get; set; }
    public string? LocalBairro          { get; set; }
    public string? LocalCidade          { get; set; }
    public string? LocalEstado          { get; set; }
}