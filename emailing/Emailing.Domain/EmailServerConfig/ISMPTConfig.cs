namespace Emailing.Domain.EmailServerConfig;
public interface ISMPTConfig
{
    public void SendEmail(string recipient, string subject, string body);
}
