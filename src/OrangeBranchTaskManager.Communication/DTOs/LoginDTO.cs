using System.ComponentModel.DataAnnotations;

namespace OrangeBranchTaskManager.Communication.DTOs;

public record LoginDTO {
    //[StringLength(100)]
    //[EmailAddress]
    public required string Email { get; set; }

    //[StringLength(100)]
    public required string Password { get; set; }
}