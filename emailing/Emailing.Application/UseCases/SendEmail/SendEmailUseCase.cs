using Emailing.Communication.Templates;
using Emailing.Domain.Enums;
using System.Text.Json;
using System.Net.Mail;
using Emailing.Domain.EmailServerConfig;
using System.Threading.Tasks;

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
        string subject = "[Orange Tasks] Bem Vindo(a) ao Orange Tasks!";
        var emailBody = $@"<!DOCTYPE html>
            <html>
            <body>
                <p>Olá, {emailInfo.Username}!</p>
                </br>
                <p>Estamos muito felizes em tê-lo(a) conosco no Orange Tasks!</p>
                <p>Aqui estão algumas dicas para começar: </p>
                <p>Crie suas primeiras tarefas e organize seu dia. Explore nossas funcionalidades para gerenciar suas tarefas de forma eficiente. Se precisar de qualquer ajuda, não hesite em entrar em contato conosco.</p>
                <p>Bem vindo(a) a bordo!</p>
                </br>
                <p>Atenciosamente, equipe Orange Tasks</p>
            </body>
            </html>
        ";

        await _smtpConfig.SendEmailAsync(emailInfo.Email, subject, emailBody);

        Console.WriteLine("Novo usuário criado!");
    }

    private async Task SendNewTaskEmail(EmailTemplate emailInfo)
    {
        var subject = $"[Orange Tasks] Nova Tarefa Criada: {emailInfo.TaskTitle}";
        var emailBody = $@"<!DOCTYPE html>
            <html>
            <body>
                <p>Olá, {emailInfo.Username}!</p>
                </br>
                <p>Uma nova tarefa foi criada em sua conta no Orange Tasks.</p>
                <p>Aqui estão os detalhes da tarefa: </p>
                <p>Tarefa: {emailInfo.TaskTitle}</p>
                <p>Descrição: {emailInfo.TaskDescription}</p>
                <p>Vencimento: {emailInfo.TaskDeadline} </p>
                <p>A tarefa pode ser acessada através de nosso aplicativo.</p>
                </br>
                <p>Atenciosamente, equipe Orange Tasks</p>
            </body>
            </html>
        ";

        await _smtpConfig.SendEmailAsync(emailInfo.Email, subject, emailBody);

        Console.WriteLine("Nova tarefa criada!");
    }

    private async Task SendUpdatedTaskEmail(EmailTemplate emailInfo)
    {
        var subject = $"[Orange Tasks] Tarefa atualizada: {emailInfo.TaskTitle}";
        var emailBody = $@"<!DOCTYPE html>
            <html>
            <body>
                <p>Olá, {emailInfo.Username}!</p>
                </br>
                <p>A tarefa a seguir foi atualizada em sua conta no Orange Tasks: </p>
                <p>Tarefa: {emailInfo.TaskTitle}</p>
                <p>Descrição: {emailInfo.TaskDescription}</p>
                <p>Vencimento: {emailInfo.TaskDeadline} </p>
                <p>A tarefa pode ser acessada através de nosso aplicativo.</p>
                </br>
                <p>Atenciosamente, equipe Orange Tasks</p>
            </body>
            </html>
        ";

        await _smtpConfig.SendEmailAsync(emailInfo.Email, subject, emailBody);

        Console.WriteLine("Uma tarefa foi atualizada!");
    }

    private async Task SendDeletedTaskEmail(EmailTemplate emailInfo)
    {
        var subject = $"[Orange Tasks] Tarefa excluída: {emailInfo.TaskTitle}";
        var emailBody = $@"<!DOCTYPE html>
            <html>
            <body>
                <p>Olá, {emailInfo.Username}!</p>
                </br>
                <p>A tarefa a seguir foi excluída de sua conta no Orange Tasks: </p>
                <p>Tarefa: {emailInfo.TaskTitle}</p>
                <p>Se essa exclusão foi um engano, a tarefa pode ser recriada em nosso aplicativo.</p>
                </br>
                <p>Atenciosamente, equipe Orange Tasks</p>
            </body>
            </html>
        ";

        await _smtpConfig.SendEmailAsync(emailInfo.Email, subject, emailBody);

        Console.WriteLine("Uma tarefa foi excluída!");
    }

}
