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
        _port = 465;
        _senderEmail = "tasksorange@gmail.com";
        _senderPassword = "SMTPPassword123!";
    }

    // WiP - There is an error while sending the email
    public void SendEmail(string recipient, string subject, string body)
    {
        using (var client = new SmtpClient(_smtpServer, _port))
        {
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential(_senderEmail, _senderPassword);

            var message = new MailMessage(_senderEmail, recipient, subject, body);
            message.IsBodyHtml = true;

            client.Send(message);
        }
    }
}
