using OrangeBranchTaskManager.Application.UseCases.CurrentUser;
using OrangeBranchTaskManager.Application.UseCases.PublishMessage;
using OrangeBranchTaskManager.Communication.DTOs;
using OrangeBranchTaskManager.Communication.Templates;
using OrangeBranchTaskManager.Domain.RabbitMQConnectionManager;
using System.Text.Json;

namespace OrangeBranchTaskManager.Application.UseCases.SendEmail;

public class SendEmailUseCase : ISendEmailUseCase
{
    private readonly IRabbitMQConnectionManager _connectionManager;
    private readonly ICurrentUserService _currentUserService;

    public SendEmailUseCase(IRabbitMQConnectionManager connectionManager, ICurrentUserService currentUserService)
    {
        _connectionManager = connectionManager;
        _currentUserService = currentUserService;
    }

    private async Task Publish(EmailTemplate messageInfo)
    {
        var messageJson = JsonSerializer.Serialize(messageInfo);

        var message = new PublishMessageDTO
        {
            Message = messageJson
        };

        var publishMessageUseCase = new PublishMessageUseCase(_connectionManager);

        await publishMessageUseCase.Execute(message);
    }
    
    public async Task CreateTaskExecute(TaskDTO task)
    {
        var messageInfo = new EmailTemplate
        {
            NotificationType = Domain.Enums.NotificationType.NewTask,
            Username = _currentUserService.GetUsername(),
            Email = _currentUserService.GetEmail(),
            TaskTitle = task.Title,
            TaskDescription = task.Description,
            TaskDeadline = task.DueDate,
        };

        await Publish(messageInfo);
    }

    public async Task DeleteTaskExecute(string taskTitle)
    {
        var messageInfo = new EmailTemplate
        {
            NotificationType = Domain.Enums.NotificationType.DeletedTask,
            Username = _currentUserService.GetUsername(),
            Email = _currentUserService.GetEmail(),
            TaskTitle = taskTitle
        };

        await Publish(messageInfo);
    }

    public async Task UpdateTaskExecute(TaskDTO task)
    {
        var messageInfo = new EmailTemplate
        {
            NotificationType = Domain.Enums.NotificationType.UpdatedTask,
            Username = _currentUserService.GetUsername(),
            Email = _currentUserService.GetEmail(),
            TaskTitle = task.Title,
            TaskDescription = task.Description,
            TaskDeadline = task.DueDate,
        };
        
        await Publish(messageInfo);
    }
}
