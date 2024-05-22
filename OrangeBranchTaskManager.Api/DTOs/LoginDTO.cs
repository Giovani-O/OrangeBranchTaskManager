using System.ComponentModel.DataAnnotations;

namespace OrangeBranchTaskManager.Api.DTOs;

public class LoginDTO
{
    [Required]
    [StringLength(100, MinimumLength = 10)]
    [EmailAddress]
    public string? Email { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 6)]
    public string? Password { get; set; }
}
