using OrangeBranchTaskManager.Domain.Entities;
using OrangeBranchTaskManager.Infrastructure.Context;

namespace OrangeBranchTaskManager.Infrastructure.Repositories.Task;

public class TaskRepository : Repository<TaskModel>, ITaskRepository
{
    public TaskRepository(AppDbContext context) : base(context)
    {
    }
}
