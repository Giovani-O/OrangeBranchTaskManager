using Microsoft.Extensions.DependencyInjection;
using OrangeBranchTaskManager.Domain.Repositories.Tasks;
using OrangeBranchTaskManager.Domain.UnitOfWork;
using OrangeBranchTaskManager.Infrastructure.Repositories.Task;
using OrangeBranchTaskManager.Infrastructure.UnitOfWork;

namespace OrangeBranchTaskManager.Infrastructure;

public static class DependencyInjectionExtension
{
    public static void AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<ITaskRepository, TaskRepository>();
        services.AddScoped<IUnitOfWork, UoW>();
    }
}
