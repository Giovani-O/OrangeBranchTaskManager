using OrangeBranchTaskManager.Communication.DTOs;

namespace OrangeBranchTaskManager.Application.UseCases.Tasks.GetById;

public interface IGetTaskByIdUseCase
{
    Task<TaskDTO> Execute(int id);
}