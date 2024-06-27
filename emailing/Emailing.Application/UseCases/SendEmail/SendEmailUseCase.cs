using Emailing.Communication.Templates;
using Emailing.Domain.Enums;
using System.Text.Json;
using System.Net.Mail;
using Emailing.Domain.EmailServerConfig;

namespace Emailing.Application.UseCases.SendEmail;

public class SendEmailUseCase : ISendEmailUseCase
{
    private readonly ISMPTConfig _smtpConfig;

    public SendEmailUseCase(ISMPTConfig smtpConfig)
    {
        _smtpConfig = smtpConfig;
    }

    public void Execute(string message)
    {
        var emailInfo = JsonSerializer.Deserialize<EmailTemplate>(message);

        if (emailInfo is null) throw new ArgumentNullException(nameof(emailInfo));

        switch (emailInfo.NotificationType)
        {
            case NotificationTypes.NewUser:
                SendNewUserEmail(emailInfo);
                break;
            case NotificationTypes.NewTask:
                SendNewTaskEmail(emailInfo);
                break;
            case NotificationTypes.UpdatedTask:
                SendUpdatedTaskEmail(emailInfo);
                break;
            case NotificationTypes.DeletedTask:
                SendDeletedTaskEmail(emailInfo);
                break;
            default:
                throw new ArgumentException("Notificação desconhecida");
        }
    }

    private void SendNewUserEmail(EmailTemplate emailInfo)
    {
        _smtpConfig.SendEmail(emailInfo.Email, "[Orange Tasks] Bem Vindo(a)!", "Email automático, não responda.");

        Console.WriteLine("Novo usuário criado!");
    }

    private void SendNewTaskEmail(EmailTemplate emailInfo)
    {
        _smtpConfig.SendEmail(emailInfo.Email, "[Orange Tasks] Nova Tarefa!", "Email automático, não responda.");

        Console.WriteLine("Nova tarefa criada!");
    }

    private void SendUpdatedTaskEmail(EmailTemplate emailInfo)
    {
        _smtpConfig.SendEmail(emailInfo.Email, "[Orange Tasks] Tarefa Atualizada!", "Email automático, não responda.");

        Console.WriteLine("Uma tarefa foi atualizada!");
    }

    private void SendDeletedTaskEmail(EmailTemplate emailInfo)
    {
        _smtpConfig.SendEmail(emailInfo.Email, "[Orange Tasks] Tarefa Excluída!", "Email automático, não responda.");

        Console.WriteLine("Uma tarefa foi excluída!");
    }
}
