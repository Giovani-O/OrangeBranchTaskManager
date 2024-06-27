namespace Emailing.Domain.EmailServerConfig;
public interface ISMPTConfig
{
    public Task SendEmailAsync(string recipient, string subject, string body);
}
