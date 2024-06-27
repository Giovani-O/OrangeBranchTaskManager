using AutoMapper;
using OrangeBranchTaskManager.Application.UseCases.CurrentUser;
using OrangeBranchTaskManager.Application.UseCases.SendMessage;
using OrangeBranchTaskManager.Communication.DTOs;
using OrangeBranchTaskManager.Communication.Templates;
using OrangeBranchTaskManager.Domain.Entities;
using OrangeBranchTaskManager.Domain.RabbitMQConnectionManager;
using OrangeBranchTaskManager.Domain.UnitOfWork;
using OrangeBranchTaskManager.Exception;
using OrangeBranchTaskManager.Exception.ExceptionsBase;
using System.Text.Json;

namespace OrangeBranchTaskManager.Application.UseCases.Tasks.Update;

public class UpdateTaskUseCase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IRabbitMQConnectionManager _connectionManager;
    private readonly ICurrentUserService _currentUserService;

    public UpdateTaskUseCase(
        IUnitOfWork unitOfWork, 
        IMapper mapper, 
        IRabbitMQConnectionManager connectionManager,
        ICurrentUserService currentUserService
    )
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _connectionManager = connectionManager;
        _currentUserService = currentUserService;
    }

    public async Task<TaskDTO> Execute(int id, TaskDTO taskData)
    {
        Validate(taskData);

        if (id <= 0 || id != taskData.Id) throw new ErrorOnExecutionException(
            new Dictionary<string, List<string>>()
            {
                { nameof(TaskDTO.Id), new List<string>() { ResourceErrorMessages.ERROR_ID_DOESNT_MATCH }}
            }
        );

        var existingTask = await _unitOfWork.TaskRepository.GetByIdAsync(id);
        if (existingTask is null) throw new ErrorOnExecutionException(
            new Dictionary<string, List<string>>()
            {
                { "Error", new List<string>() { ResourceErrorMessages.ERROR_NOT_FOUND_TASK } }
            }
        );

        _mapper.Map(taskData, existingTask);

        _unitOfWork.TaskRepository.UpdateAsync(existingTask);
        await _unitOfWork.CommitAsync();

        var result = _mapper.Map<TaskDTO>(existingTask);

        await SendMessage(taskData);

        return result;
    }

    private void Validate(TaskDTO request)
    {
        var validator = new UpdateTaskValidator();
        var result = validator.Validate(request);

        if (!result.IsValid)
        {
            var errorDictionary = result.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(x => x.Key, x => x.Select(e => e.ErrorMessage).ToList());

            throw new ErrorOnValidationException(errorDictionary);
        }
    }

    private async Task SendMessage(TaskDTO task)
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

        var messageJson = JsonSerializer.Serialize(messageInfo);

        var message = new SendMessageDTO
        {
            Message = messageJson
        };

        var sendMessageUseCase = new SendMessageUseCase(_connectionManager);

        await sendMessageUseCase.Execute(message);
    }
}
