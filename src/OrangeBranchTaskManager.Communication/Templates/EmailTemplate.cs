using OrangeBranchTaskManager.Domain.Enums;

namespace OrangeBranchTaskManager.Communication.Templates;

public record EmailTemplate
{
    public required NotificationType NotificationType { get; set; }
    public required string? Username { get; set; }
    public required string? Email { get; set; }
    public string TaskTitle { get; set; } = string.Empty;
    public string TaskDescription { get; set; } = string.Empty;
    public DateTime TaskDeadline { get; set; }
}
