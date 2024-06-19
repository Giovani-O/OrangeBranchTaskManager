using System.ComponentModel.DataAnnotations;

namespace OrangeBranchTaskManager.Api.DTOs;

public class UserDTO
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string? Username { get; set; }

    [Required]
    [StringLength(100)]
    [EmailAddress]
    public string? Email { get; set; }
}
