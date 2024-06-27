using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

namespace Emailing.Domain.EmailServerConfig;

public class SMTPConfig : ISMPTConfig
{
    private string _smtpServer;
    private int _port;
    private string _senderEmail;
    private string _senderPassword;

    public SMTPConfig(/*IConfiguration configuration*/)
    {
        //_smtpServer = configuration["EmailServer:SMTPServer"]!;
        //_port = int.Parse(configuration["EmailServer:Port"]!);
        //_senderEmail = configuration["EmailServer:SenderEmail"]!;
        //_senderPassword = configuration["EmailServer:SenderPassword"]!;
        _smtpServer = "smtp.gmail.com";
        _port = 587;
        _senderEmail = "";
        _senderPassword = "";
    }

    // WiP - There is an error while sending the email
    public async Task SendEmailAsync(string recipient, string subject, string body)
    {
        var client = new SmtpClient(_smtpServer, _port)
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
