using System.ComponentModel.DataAnnotations;

namespace OrangeBranchTaskManager.Api.DTOs
{
    public class TaskDTO
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string? Title { get; set; }

        [StringLength(300)]
        public string? Description { get; set; }

        [Required]
        public DateTime DueDate { get; set;}
    }
}
