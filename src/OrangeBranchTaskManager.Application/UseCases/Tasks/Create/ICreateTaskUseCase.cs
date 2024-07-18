using OrangeBranchTaskManager.Communication.DTOs;

namespace OrangeBranchTaskManager.Application.UseCases.Tasks.Create;

public interface ICreateTaskUseCase
{
    Task<TaskDTO> Execute(TaskDTO taskData);
}