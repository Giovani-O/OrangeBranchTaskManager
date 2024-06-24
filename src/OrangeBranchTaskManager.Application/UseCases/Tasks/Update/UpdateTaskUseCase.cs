using AutoMapper;
using OrangeBranchTaskManager.Application.UseCases.SendMessage;
using OrangeBranchTaskManager.Communication.DTOs;
using OrangeBranchTaskManager.Domain.RabbitMQConnectionManager;
using OrangeBranchTaskManager.Domain.UnitOfWork;
using OrangeBranchTaskManager.Exception;
using OrangeBranchTaskManager.Exception.ExceptionsBase;

namespace OrangeBranchTaskManager.Application.UseCases.Tasks.Update;

public class UpdateTaskUseCase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IRabbitMQConnectionManager _connectionManager;

    public UpdateTaskUseCase(
        IUnitOfWork unitOfWork, 
        IMapper mapper, 
        IRabbitMQConnectionManager connectionManager
    )
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _connectionManager = connectionManager;
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

        await SendMessage();

        return result;
    }

    private void Validate(TaskDTO request)
    {
        var validator = new UpdateTaskValidator();
        var result = validator.Validate(request);

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

    private async Task SendMessage()
    {
        var message = new SendMessageDTO
        {
            Message = "Tarefa atualizada"
        };

        var sendMessageUseCase = new SendMessageUseCase(_connectionManager);

        await sendMessageUseCase.Execute(message);
    }
}
