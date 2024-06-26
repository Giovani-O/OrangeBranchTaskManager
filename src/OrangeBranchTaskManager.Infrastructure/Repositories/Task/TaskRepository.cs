﻿using OrangeBranchTaskManager.Domain.Entities;
using OrangeBranchTaskManager.Domain.Repositories.Tasks;
using OrangeBranchTaskManager.Infrastructure.Context;

namespace OrangeBranchTaskManager.Infrastructure.Repositories.Task;

internal class TaskRepository : Repository<TaskModel>, ITaskRepository
{
    public TaskRepository(AppDbContext context) : base(context)
    {
    }
}
