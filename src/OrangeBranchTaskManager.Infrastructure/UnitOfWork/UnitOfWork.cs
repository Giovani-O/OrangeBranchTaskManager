﻿using OrangeBranchTaskManager.Domain.Repositories.Tasks;
using OrangeBranchTaskManager.Domain.UnitOfWork;
using OrangeBranchTaskManager.Infrastructure.Context;
using OrangeBranchTaskManager.Infrastructure.Repositories.Task;

namespace OrangeBranchTaskManager.Infrastructure.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private ITaskRepository? _taskRepository;
    public AppDbContext _context;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    public ITaskRepository TaskRepository
    {
        get
        {
            return _taskRepository = _taskRepository ?? new TaskRepository(_context);
        }
    }

    public Task CommitAsync()
    {
        return _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
