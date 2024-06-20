using System.ComponentModel.DataAnnotations;

namespace OrangeBranchTaskManager.Communication.DTOs;

public record RegisterDTO
{
    //[StringLength(100)]
    public required string Username { get; init; }

    //[StringLength(100)]
    //[EmailAddress]
    public required string Email { get; init; }

    //[StringLength(100)]
    public required string Password { get; init; }
}
