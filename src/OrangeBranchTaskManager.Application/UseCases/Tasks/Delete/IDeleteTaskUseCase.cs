using OrangeBranchTaskManager.Communication.DTOs;

namespace OrangeBranchTaskManager.Application.UseCases.Tasks.Delete;

public interface IDeleteTaskUseCase
{
    Task<TaskDTO> Execute(int id);
}