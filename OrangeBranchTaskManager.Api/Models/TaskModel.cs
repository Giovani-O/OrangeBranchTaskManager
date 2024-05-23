using System.ComponentModel.DataAnnotations;

namespace OrangeBranchTaskManager.Api.Models;

public class TaskModel
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public DateTime DueDate { get; set; }
}
