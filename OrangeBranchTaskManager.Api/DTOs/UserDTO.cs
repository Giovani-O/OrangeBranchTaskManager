using System.ComponentModel.DataAnnotations;

namespace OrangeBranchTaskManager.Api.DTOs;

public class UserDTO
{
    public int Id { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 3)]
    public string? Username { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 10)]
    [EmailAddress]
    public string? Email { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 6)]
    public string? Password { get; set; }
}
