using OrangeBranchTaskManager.Infrastructure.Repositories.Task;

namespace OrangeBranchTaskManager.Infrastructure.UnitOfWork;
public interface IUnitOfWork
{
    ITaskRepository TaskRepository { get; }

    Task CommitAsync();
}
