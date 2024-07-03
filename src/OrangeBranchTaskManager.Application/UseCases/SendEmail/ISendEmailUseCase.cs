using OrangeBranchTaskManager.Communication.DTOs;

namespace OrangeBranchTaskManager.Application.UseCases.SendEmail;
public interface ISendEmailUseCase
{
    Task CreateTaskExecute(TaskDTO task);
    Task DeleteTaskExecute(string taskTitle);
    Task UpdateTaskExecute(TaskDTO task);
}
