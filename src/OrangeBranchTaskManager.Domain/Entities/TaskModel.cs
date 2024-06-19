using System.ComponentModel.DataAnnotations;

namespace OrangeBranchTaskManager.Domain.Entities;

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
