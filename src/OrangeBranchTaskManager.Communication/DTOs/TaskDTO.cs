using System.ComponentModel.DataAnnotations;

namespace OrangeBranchTaskManager.Communication.DTOs;

public record TaskDTO
{
    public int Id { get; init; }

    [StringLength(100)]
    public required string Title { get; init; }

    [StringLength(300)]
    public required string Description { get; init; }

    public required DateTime DueDate { get; init; }
}
