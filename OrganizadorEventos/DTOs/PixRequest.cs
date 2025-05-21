namespace OrganizadorEventos.DTOs;

public class PixRequest
{
    public string PixKey { get; set; } = string.Empty;
    public string ReceiverName { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string? Message { get; set; }
    public string? TransactionId { get; set; }
}