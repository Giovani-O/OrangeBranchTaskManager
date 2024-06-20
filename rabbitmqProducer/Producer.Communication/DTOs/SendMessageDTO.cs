namespace Producer.Communication.DTOs;

public record SendMessageDTO
{
    public required string Message { get; init; }
}
