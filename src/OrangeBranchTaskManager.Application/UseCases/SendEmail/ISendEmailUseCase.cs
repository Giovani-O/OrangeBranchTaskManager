using OrangeBranchTaskManager.Communication.DTOs;

namespace OrangeBranchTaskManager.Application.UseCases.SendEmail;
public interface ISendEmailUseCase
{
    Task Execute(TaskDTO task);
}
