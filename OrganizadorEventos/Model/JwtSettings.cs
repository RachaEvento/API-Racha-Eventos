namespace OrganizadorEventos.Model;

public class JwtSettings
{
    public string SecretKey { get; set; }
    public int ExpiryMinutes { get; set; }
}