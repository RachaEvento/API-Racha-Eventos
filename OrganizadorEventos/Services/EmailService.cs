using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using OrganizadorEventos.DTOs.Email;
using OrganizadorEventos.Interfaces.Services;
using OrganizadorEventos.Model;

namespace OrganizadorEventos.Services;

public class EmailService : IEmailService
{
    private readonly EmailSettings _emailSettings;

    public EmailService(IOptions<EmailSettings> emailSettings)
    {
        _emailSettings = emailSettings.Value;
    }

    public async Task SendInvitationEmailAsync(ConviteEmailDTO convite)
    {
        var mailMessage = new MailMessage
        {
            From = new MailAddress(_emailSettings.Username),
            Subject = $"Convite para {convite.EventoNome}",
            IsBodyHtml = true
        };

        mailMessage.To.Add(convite.ToEmail);

        string body = GetInvitationEmailBody(
            convite.ConvidadoNome, 
            convite.QuemConvidaNome, 
            convite.EventoNome, 
            convite.EventoData, 
            $"{_emailSettings.ConvidarUrl}/{convite.CodigoConfirmacao}");

        var avHtml = AlternateView.CreateAlternateViewFromString(body, null, "text/html");

        mailMessage.AlternateViews.Add(avHtml);

        using (var smtpClient = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.Port))
        {
            smtpClient.Credentials = new NetworkCredential(_emailSettings.Username, _emailSettings.Password);
            smtpClient.EnableSsl = _emailSettings.EnableSsl;

            await smtpClient.SendMailAsync(mailMessage);
        }
    }

    public async Task SendChargeEmailAsync(CobrancaEmailDTO cobranca)
    {
        var mailMessage = new MailMessage
        {
          From = new MailAddress(_emailSettings.Username),
          Subject = $"Cobrança do evento {cobranca.EventoNome}",
          IsBodyHtml = true,
        };
          
        mailMessage.To.Add(cobranca.ToEmail);

        string body = GetChargeEmailBody(
          cobranca.ConvidadoNome, 
          cobranca.QuemConvidaNome, 
          cobranca.EventoNome, 
          cobranca.Valor, 
          $"{_emailSettings.CobrarUrl}/{cobranca.CodigoParticipante}");

        AlternateView avHtml = AlternateView.CreateAlternateViewFromString(body, null, "text/html");

        mailMessage.AlternateViews.Add(avHtml);

        using (var smtpClient = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.Port))
        {
          smtpClient.Credentials = new NetworkCredential(_emailSettings.Username, _emailSettings.Password);
          smtpClient.EnableSsl = _emailSettings.EnableSsl;
              
          await smtpClient.SendMailAsync(mailMessage);
        }
    }
    
    private string GetChargeEmailBody(
      string convidadoNome,
      string quemConvidaNome,
      string eventoNome,
      decimal eventoValor,
      string linkPagamento)
    {
      return $@"<!DOCTYPE html>
      <html>
      <head>
        <meta http-equiv=""Content-Type"" content=""text/html; charset=UTF-8""/>
      </head>
      <body style=""margin:0; padding:0; background-color:#f4f4f4;"">
        <table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0"" bgcolor=""#f4f4f4"">
          <tr>
            <td align=""center"">
              <table width=""600"" cellpadding=""0"" cellspacing=""0"" border=""0"" style=""background-color:#ffffff; border-radius:8px; overflow:hidden;"">
                
                <!-- Logo -->
                <tr>
                  <td align=""center"" style=""padding:20px; font-family:Arial,sans-serif;"">
                    <h1 style=""margin: 0; font-size: 24px;"">Racha Eventos</h1>
                  </td>
                </tr>
                
                <!-- Greeting & Details -->
                <tr>
                  <td style=""padding:0 20px 20px 20px; font-family:Arial,sans-serif; color:#333333;"">
                    <h2 style=""margin:0 0 10px 0; font-size:24px;"">Olá, {convidadoNome}!</h2>
                    <p style=""margin:0 0 10px 0; font-size:16px; color:#555555;"">
                      {quemConvidaNome} está esperando seu pagamento sobre o evento <strong>{eventoNome}</strong>!
                    </p>
                    <p style=""margin:0; font-size:16px; color:#555555;"">
                      <strong>Valor a pagar:</strong> {eventoValor}
                    </p>
                  </td>
                </tr>
                
                <!-- Button -->
                <tr>
                  <td align=""center"" style=""padding:20px;"">
                    <a href=""{linkPagamento}"" 
                       style=""display:inline-block; font-family:Arial,sans-serif; font-size:16px; color:#ffffff; background-color:#4CAF50; text-decoration:none; padding:15px 25px; border-radius:5px;"">
                      Enviar Pagamento
                    </a>
                  </td>
                </tr>
                
                <!-- Footer -->
                <tr>
                  <td style=""padding:0 20px 20px 20px; font-family:Arial,sans-serif; font-size:12px; color:#999999; text-align:center;"">
                    Se você não reconhece esta cobrança, pode ignorar este e-mail.
                  </td>
                </tr>
                
              </table>
            </td>
          </tr>
        </table>
      </body>
      </html>";
    }

    private string GetInvitationEmailBody(
      string convidadoNome,
      string quemConvidaNome,
      string eventoNome,
      DateTime eventoData,
      string linkConfirmacao)
    {
        return $@"<!DOCTYPE html>
      <html>
      <head>
        <meta http-equiv=""Content-Type"" content=""text/html; charset=UTF-8""/>
      </head>
      <body style=""margin:0; padding:0; background-color:#f4f4f4;"">
        <table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0"" bgcolor=""#f4f4f4"">
          <tr>
            <td align=""center"">
              <table width=""600"" cellpadding=""0"" cellspacing=""0"" border=""0"" style=""background-color:#ffffff; border-radius:8px; overflow:hidden;"">
                
                <!-- Logo -->
                <tr>
                  <td align=""center"" style=""padding:20px; font-family:Arial,sans-serif;"">
                    <h1 style=""margin: 0; font-size: 24px;"">Racha Eventos</h1>
                  </td>
                </tr>
                
                <!-- Greeting & Details -->
                <tr>
                  <td style=""padding:0 20px 20px 20px; font-family:Arial,sans-serif; color:#333333;"">
                    <h2 style=""margin:0 0 10px 0; font-size:24px;"">Olá, {convidadoNome}!</h2>
                    <p style=""margin:0 0 10px 0; font-size:16px; color:#555555;"">
                      {quemConvidaNome} está convidando você para o evento <strong>{eventoNome}</strong>!
                    </p>
                    <p style=""margin:0; font-size:16px; color:#555555;"">
                      <strong>Data do Evento:</strong> {eventoData:dd/MM/yyyy HH:mm}
                    </p>
                  </td>
                </tr>
                
                <!-- Button -->
                <tr>
                  <td align=""center"" style=""padding:20px;"">
                    <a href=""{linkConfirmacao}"" 
                       style=""display:inline-block; font-family:Arial,sans-serif; font-size:16px; color:#ffffff; background-color:#4CAF50; text-decoration:none; padding:15px 25px; border-radius:5px;"">
                      Confirmar Presença
                    </a>
                  </td>
                </tr>
                
                <!-- Footer -->
                <tr>
                  <td style=""padding:0 20px 20px 20px; font-family:Arial,sans-serif; font-size:12px; color:#999999; text-align:center;"">
                    Se você não reconhece este convite, pode ignorar este e-mail.
                  </td>
                </tr>
                
              </table>
            </td>
          </tr>
        </table>
      </body>
      </html>";
    }
}