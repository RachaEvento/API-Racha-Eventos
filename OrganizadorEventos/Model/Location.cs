namespace OrganizadorEventos.Model;

public class Location
{
    public Guid Id { get; set; }
    public string Pais { get; set; }
    public string Estado { get; set; }
    public string Cidade { get; set; }
    public string Rua { get; set; }
    public string DescricaoLocal { get; set; }
}