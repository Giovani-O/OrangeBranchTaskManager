namespace OrangeBranchTaskManager.Api.Repositories;

public interface IRepository<T>
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> GetByIdAsync(int id);
    T CreateAsync(T entity);
    T UpdateAsync(T entity);
    T DeleteAsync(T entity);
}
