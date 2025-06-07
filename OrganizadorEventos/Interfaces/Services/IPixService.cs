namespace OrganizadorEventos.Interfaces.Services;

public interface IPixService
{
    Task<string> GeneratePixQrCodeAsync(string pixKey, string receiverName, string city, decimal amount,
        string? message, string? transactionId);
}