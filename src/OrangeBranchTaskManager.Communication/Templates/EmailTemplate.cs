using OrangeBranchTaskManager.Domain.Enums;

namespace OrangeBranchTaskManager.Communication.Templates;

public record EmailTemplate
{
    public required NotificationType NotificationType { get; init; }
    public required string Username { get; init; }
    public required string Email { get; init; }
    public string TaskTitle { get; init; }
    public string TaskDescription { get; init; }
    public DateTime TaskDeadline { get; init; }
}
