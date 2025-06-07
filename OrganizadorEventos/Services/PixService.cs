using System.Globalization;
using Newtonsoft.Json;
using OrganizadorEventos.DTOs;
using OrganizadorEventos.Interfaces.Services;

namespace OrganizadorEventos.Services;

public class PixService : IPixService
{
    private readonly HttpClient _httpClient;

    public PixService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> GeneratePixQrCodeAsync(string pixKey, string receiverName, string city, decimal amount,
        string? message, string? transactionId)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "https://www.invertexto.com/qrcode-pix/generate.php");

        var formData = new MultipartFormDataContent
        {
            { new StringContent(pixKey), "chave" },
            { new StringContent(receiverName), "nomebeneficiario" },
            { new StringContent(city), "cidadebeneficiario" },
            { new StringContent(amount.ToString("F2", CultureInfo.InvariantCulture)), "valor" },
            { new StringContent(message ?? string.Empty), "descricao" },
            { new StringContent(transactionId ?? string.Empty), "identificador" }
        };

        request.Content = formData;

        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<PixResponse>(json);

        return result.payload;
    }
}