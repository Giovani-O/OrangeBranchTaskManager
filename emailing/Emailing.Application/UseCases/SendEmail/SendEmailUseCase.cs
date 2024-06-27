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

    public async Task Execute(string message)
    {
        var emailInfo = JsonSerializer.Deserialize<EmailTemplate>(message);

        if (emailInfo is null) throw new ArgumentNullException(nameof(emailInfo));

        switch (emailInfo.NotificationType)
        {
            case NotificationTypes.NewUser:
                await SendNewUserEmail(emailInfo);
                break;
            case NotificationTypes.NewTask:
                await SendNewTaskEmail(emailInfo);
                break;
            case NotificationTypes.UpdatedTask:
                await SendUpdatedTaskEmail(emailInfo);
                break;
            case NotificationTypes.DeletedTask:
                await SendDeletedTaskEmail(emailInfo);
                break;
            default:
                throw new ArgumentException("Notificação desconhecida");
        }
    }

    private async Task SendNewUserEmail(EmailTemplate emailInfo)
    {
        await _smtpConfig.SendEmailAsync(emailInfo.Email, "[Orange Tasks] Bem Vindo(a)!", "Email automático, não responda.");

        Console.WriteLine("Novo usuário criado!");
    }

    private async Task SendNewTaskEmail(EmailTemplate emailInfo)
    {
        await _smtpConfig.SendEmailAsync(emailInfo.Email, "[Orange Tasks] Nova Tarefa!", "Email automático, não responda.");

        Console.WriteLine("Nova tarefa criada!");
    }

    private async Task SendUpdatedTaskEmail(EmailTemplate emailInfo)
    {
        await _smtpConfig.SendEmailAsync(emailInfo.Email, "[Orange Tasks] Tarefa Atualizada!", "Email automático, não responda.");

        Console.WriteLine("Uma tarefa foi atualizada!");
    }

    private async Task SendDeletedTaskEmail(EmailTemplate emailInfo)
    {
        await _smtpConfig.SendEmailAsync(emailInfo.Email, "[Orange Tasks] Tarefa Excluída!", "Email automático, não responda.");

        Console.WriteLine("Uma tarefa foi excluída!");
    }

}
