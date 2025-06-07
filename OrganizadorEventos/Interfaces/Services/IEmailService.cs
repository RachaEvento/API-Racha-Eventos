using OrganizadorEventos.DTOs.Email;

namespace OrganizadorEventos.Interfaces.Services;

public interface IEmailService
{
    Task SendInvitationEmailAsync(ConviteEmailDTO convite);
    Task SendChargeEmailAsync(CobrancaEmailDTO convite);
}