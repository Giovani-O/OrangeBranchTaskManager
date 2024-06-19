using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrangeBranchTaskManager.Infrastructure.Repositories;
public interface IRepository<T>
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> GetByIdAsync(int id);
    T CreateAsync(T entity);
    T UpdateAsync(T entity);
    T DeleteAsync(T entity);
}
