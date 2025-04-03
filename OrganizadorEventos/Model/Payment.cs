namespace OrganizadorEventos.Model;

public class Payment
{
    public Guid Id { get; set; }
    public Guid EventId { get; set; }
    public Guid UserId { get; set; }
    public decimal Amount { get; set; }
    public string PaymentLink { get; set; }
    public bool IsPaid { get; set; } = false;
    public DateTime PaymentDate { get; set; }
    public Event Event { get; set; }
}