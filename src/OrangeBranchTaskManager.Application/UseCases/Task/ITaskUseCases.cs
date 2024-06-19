using OrangeBranchTaskManager.Communication.DTOs;

namespace OrangeBranchTaskManager.Application.UseCases.Task;
public interface ITaskUseCases
{
    Task<IEnumerable<TaskDTO>> GetTasks();
    Task<TaskDTO> GetById(int id);
    Task<TaskDTO> CreateTask(TaskDTO taskData);
    Task<TaskDTO> UpdateTask(int id, TaskDTO taskData);
    Task<TaskDTO> DeleteTask(int id);
}
