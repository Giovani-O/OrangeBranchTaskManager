using OrangeBranchTaskManager.Communication.DTOs;

namespace OrangeBranchTaskManager.Application.UseCases.Authentication.Login;

public interface ILoginUseCase
{
    public Task<LoginResponseDTO> Execute(LoginDTO loginDto);
}