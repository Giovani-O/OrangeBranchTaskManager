using OrangeBranchTaskManager.Domain.Repositories.Tasks;

namespace OrangeBranchTaskManager.Domain.UnitOfWork;
public interface IUnitOfWork
{
    ITaskRepository TaskRepository { get; }

    Task CommitAsync();
}
