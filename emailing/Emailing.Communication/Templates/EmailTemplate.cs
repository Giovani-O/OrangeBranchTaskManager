using Emailing.Domain.Enums;

namespace Emailing.Communication.Templates;

public record EmailTemplate
{
    public required NotificationTypes NotificationType { get; init; }
    public required string Username { get; init; }
    public string TaskTitle { get; init; }
    public string TaskDescription { get; init; }
    public DateTime TaskDeadline { get; init; }
}