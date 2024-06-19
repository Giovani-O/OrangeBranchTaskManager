using OrangeBranchTaskManager.Api.DTOs;

namespace OrangeBranchTaskManager.Api.Services;

public interface ITasksService
{
    Task<IEnumerable<TaskDTO>> GetTasks();
    Task<TaskDTO> GetById(int id);
    Task<TaskDTO> CreateTask(TaskDTO taskData);
    Task<TaskDTO> UpdateTask(int id, TaskDTO taskData);
    Task<TaskDTO> DeleteTask(int id);
}
