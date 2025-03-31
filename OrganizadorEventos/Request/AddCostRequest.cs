namespace OrganizadorEventos.Request;

public class AddCostRequest
{
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
}