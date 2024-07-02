namespace OrangeBranchTaskManager.Communication.DTOs;

public record PublishMessageDTO
{
    public required string Message { get; init; }
}
