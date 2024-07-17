using Microsoft.AspNetCore.Identity;
using OrangeBranchTaskManager.Communication.DTOs;

namespace OrangeBranchTaskManager.Application.UseCases.Authentication.Register;

public interface IRegisterUseCase
{
    public Task<IdentityResult> Execute(RegisterDTO registerDto);
}