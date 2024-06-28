using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

namespace Emailing.Domain.EmailServerConfig;

public class SMTPConfig : ISMPTConfig
{
    private string _smtpServer;
    private string _port;
    private string _senderEmail;
    private string _senderPassword;

    // É necessário configurar as variáveis de ambiente para enviar emails
    // Lembre-se de reiniciar o Visual Studio após configurar as variáveis de ambiente.
    /*
     * Eu dou preferência para o Gmail, então o smtp server deve ser da Google
     * A porta usada é TLS
     * No sender email, dê preferência a um Gmail
     * Em sender password, é necessário gerar uma senha de aplicativo em sua conta, ao invés de usar sua senha de acesso
     */
    public SMTPConfig()
    {
        _smtpServer = Environment.GetEnvironmentVariable("ORANGE_TASKS_SMTP_SERVER")!;
        _port = Environment.GetEnvironmentVariable("ORANGE_TASKS_SMTP_PORT")!;
        _senderEmail = Environment.GetEnvironmentVariable("ORANGE_TASKS_SENDER_EMAIL")!;
        _senderPassword = Environment.GetEnvironmentVariable("ORANGE_TASKS_SENDER_APP_PASSWORD")!;
    }

    public async Task SendEmailAsync(string recipient, string subject, string body)
    {
        var client = new SmtpClient(_smtpServer, int.Parse(_port))
        {
            EnableSsl = true,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(_senderEmail, _senderPassword)
        };

        await client.SendMailAsync(
            new MailMessage(
                    from: _senderEmail,
                    to: recipient,
                    subject,
                    body
                ));
    }
}
