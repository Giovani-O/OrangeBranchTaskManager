namespace OrangeBranchTaskManager.Api.Repositories;

public interface IUnitOfWork
{
    ITaskRepository TaskRepository { get; }

    Task CommitAsync();
}
