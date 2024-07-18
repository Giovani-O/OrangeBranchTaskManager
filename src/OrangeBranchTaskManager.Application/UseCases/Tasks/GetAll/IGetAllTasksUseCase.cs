using OrangeBranchTaskManager.Communication.DTOs;

namespace OrangeBranchTaskManager.Application.UseCases.Tasks.GetAll;

public interface IGetAllTasksUseCase
{
    Task<IEnumerable<TaskDTO>> Execute();
}