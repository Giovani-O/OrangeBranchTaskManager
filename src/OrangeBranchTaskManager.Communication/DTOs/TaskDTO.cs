using System.ComponentModel.DataAnnotations;

namespace OrangeBranchTaskManager.Communication.DTOs;

public record TaskDTO
{
    public int Id { get; set; }

    //[StringLength(100)]
    public required string Title { get; set; }

    //[StringLength(300)]
    public required string Description { get; set; }

    public required DateTime DueDate { get; set; }
}
