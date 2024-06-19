using OrangeBranchTaskManager.Api.Data;
using OrangeBranchTaskManager.Api.Models;

namespace OrangeBranchTaskManager.Api.Repositories;

public class TaskRepository : Repository<TaskModel>, ITaskRepository
{
    public TaskRepository(AppDbContext context) : base(context)
    {
    }
}
