using Emailing.Communication.Templates;
using Emailing.Domain.Enums;
using System.Text.Json;

namespace Emailing.Application.UseCases.SendEmail;

public class SendEmailUseCase : ISendEmailUseCase
{
    public void Execute(string message)
    {
        var emailInfo = JsonSerializer.Deserialize<EmailTemplate>(message);

        if (emailInfo is null) throw new ArgumentNullException(nameof(emailInfo));

        switch (emailInfo.NotificationType)
        {
            case NotificationTypes.NewUser:
                Console.WriteLine("Send new user email");
                break;
            case NotificationTypes.NewTask:
                Console.WriteLine("Send new task email");
                break;
            case NotificationTypes.UpdatedTask:
                Console.WriteLine("Send updated task email");
                break;
            case NotificationTypes.DeletedTask:
                Console.WriteLine("Send deleted task email");
                break;
            default:
                throw new ArgumentException("Notificação desconhecida");
        }
    }
}
