using Microsoft.Extensions.DependencyInjection;
using OrangeBranchTaskManager.Application.Mappings;
using OrangeBranchTaskManager.Application.UseCases.Authentication.Login;
using OrangeBranchTaskManager.Application.UseCases.Authentication.Register;
using OrangeBranchTaskManager.Application.UseCases.CurrentUser;
using OrangeBranchTaskManager.Application.UseCases.PublishMessage;
using OrangeBranchTaskManager.Application.UseCases.SendEmail;
using OrangeBranchTaskManager.Application.UseCases.Tasks.Create;
using OrangeBranchTaskManager.Application.UseCases.Tasks.Delete;
using OrangeBranchTaskManager.Application.UseCases.Tasks.GetAll;
using OrangeBranchTaskManager.Application.UseCases.Tasks.GetById;
using OrangeBranchTaskManager.Application.UseCases.Tasks.Update;
using OrangeBranchTaskManager.Application.UseCases.Token.TokenService;

namespace OrangeBranchTaskManager.Application;

public static class DependencyInjectionExtension
{
    public static void AddApplication(this IServiceCollection services)
    {
        AddAutomapper(services);
        AddUseCases(services);
    }

    private static void AddAutomapper(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(TaskDTOMappingProfile));
    }

    private static void AddUseCases(this IServiceCollection services)
    {
        services.AddScoped<ILoginUseCase, LoginUseCase>();
        services.AddScoped<IRegisterUseCase, RegisterUseCase>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<IPublishMessageUseCase, PublishMessageUseCase>();
        services.AddScoped<ISendEmailUseCase, SendEmailUseCase>();
        services.AddScoped<ICreateTaskUseCase, CreateTaskUseCase>();
        services.AddScoped<IDeleteTaskUseCase, DeleteTaskUseCase>();
        services.AddScoped<IGetAllTasksUseCase, GetAllTasksUseCase>();
        services.AddScoped<IGetTaskByIdUseCase, GetTaskByIdUseCase>();
        services.AddScoped<IUpdateTaskUseCase, UpdateTaskUseCase>();
        services.AddScoped<ITokenServiceUseCase, TokenServiceUseCase>();
    }
}