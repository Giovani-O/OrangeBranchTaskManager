using OrangeBranchTaskManager.Communication.DTOs;

namespace OrangeBranchTaskManager.Application.UseCases.Tasks.Update;

public interface IUpdateTaskUseCase
{
    Task<TaskDTO> Execute(int id, TaskDTO taskData);
}