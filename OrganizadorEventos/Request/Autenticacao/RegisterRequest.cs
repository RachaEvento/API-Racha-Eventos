namespace OrganizadorEventos.Request.Autenticacao;

public class RegisterRequest
{
    public string Nome { get; set; }
    public string Email { get; set; }
    public string Numero { get; set; }
    public string Password { get; set; }
}