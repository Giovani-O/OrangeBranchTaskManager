
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrangeBranchTaskManager.Communication.DTOs;
public record LoginResponseDTO
{

    public required string Token { get; init; }
    public required DateTime ValidTo { get; init; }
    public required string UserName { get; init; }
}
