using AutoMapper;
using OrangeBranchTaskManager.Application.UseCases.CurrentUser;
using OrangeBranchTaskManager.Application.UseCases.SendMessage;
using OrangeBranchTaskManager.Communication.DTOs;
using OrangeBranchTaskManager.Communication.Templates;
using OrangeBranchTaskManager.Domain.RabbitMQConnectionManager;
using OrangeBranchTaskManager.Domain.UnitOfWork;
using OrangeBranchTaskManager.Exception;
using OrangeBranchTaskManager.Exception.ExceptionsBase;
using System.Text.Json;
using System.Threading.Tasks;

namespace OrangeBranchTaskManager.Application.UseCases.Tasks.Delete;

public class DeleteTaskUseCase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IRabbitMQConnectionManager _connectionManager;
    private readonly ICurrentUserService _currentUserService;

    public DeleteTaskUseCase(
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

    public async Task<TaskDTO> Execute(int id)
    {
        Validate(id);

        var existingTask = await _unitOfWork.TaskRepository.GetByIdAsync(id);
        if (existingTask is null) throw new ErrorOnExecutionException(
            new Dictionary<string, List<string>>()
            {
                { "Error", new List<string>() { ResourceErrorMessages.ERROR_DELETE_TASK } }
            }
        );

        _unitOfWork.TaskRepository.DeleteAsync(existingTask);
        await _unitOfWork.CommitAsync();

        await SendMessage(existingTask.Title!);

        return _mapper.Map<TaskDTO>(existingTask);
    }

    private void Validate(int id)
    {
        var validator = new DeleteTaskValidator();
        var result = validator.Validate(id);

        if (!result.IsValid)
        {
            //var errorMessages = result.Errors.Select(x => x.ErrorMessage).ToList();
            //throw new ErrorOnValidationException(errorMessages);
            var errorDictionary = result.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(x => x.Key, x => x.Select(e => e.ErrorMessage).ToList());

            throw new ErrorOnValidationException(errorDictionary);
        }
    }

    private async Task SendMessage(string taskTitle)
    {
        var messageInfo = new EmailTemplate
        {
            NotificationType = Domain.Enums.NotificationType.DeletedTask,
            Username = _currentUserService.GetUsername(),
            Email = _currentUserService.GetEmail(),
            TaskTitle = taskTitle
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
