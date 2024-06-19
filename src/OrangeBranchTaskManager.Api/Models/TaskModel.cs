using System.ComponentModel.DataAnnotations;

namespace OrangeBranchTaskManager.Api.Models;

public class TaskModel
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string? Title { get; set; }

    [Required]
    [StringLength(400)]
    public string? Description { get; set; }

    [Required]
    public DateTime DueDate { get; set; }
}
