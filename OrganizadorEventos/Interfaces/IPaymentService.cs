using OrganizadorEventos.Model;

namespace OrganizadorEventos.Interfaces;

public interface IPaymentService
{
    Task<List<Payment>> CreatePaymentPreferencesAsync(Guid eventId);
}