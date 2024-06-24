using Microsoft.EntityFrameworkCore;
using OrangeBranchTaskManager.Domain.Repositories;
using OrangeBranchTaskManager.Infrastructure.Context;

namespace OrangeBranchTaskManager.Infrastructure.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly AppDbContext _context;

    public Repository(AppDbContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _context.Set<T>().AsNoTracking().ToListAsync();
    }

    public async Task<T> GetByIdAsync(int id)
    {
        var task = await _context.Set<T>().FindAsync(id);
        return task;
    }

    public T CreateAsync(T entity)
    {
        _context.Set<T>().Add(entity);
        return entity;
    }

    public T UpdateAsync(T entity)
    {
        _context.Set<T>().Update(entity);
        return entity;
    }

    public T DeleteAsync(T entity)
    {
        _context.Set<T>().Remove(entity);
        return entity;
    }
}
