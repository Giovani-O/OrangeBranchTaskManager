using AutoMapper;
using OrangeBranchTaskManager.Application.UseCases.SendMessage;
using OrangeBranchTaskManager.Communication.DTOs;
using OrangeBranchTaskManager.Domain.RabbitMQConnectionManager;
using OrangeBranchTaskManager.Domain.UnitOfWork;
using OrangeBranchTaskManager.Exception;
using OrangeBranchTaskManager.Exception.ExceptionsBase;

namespace OrangeBranchTaskManager.Application.UseCases.Tasks.Delete;

public class DeleteTaskUseCase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IRabbitMQConnectionManager _connectionManager;

    public DeleteTaskUseCase(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IRabbitMQConnectionManager connectionManager
    )
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _connectionManager = connectionManager;
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

        await SendMessage();

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

    private async Task SendMessage()
    {
        var message = new SendMessageDTO
        {
            Message = "Tarefa excluída"
        };

        var sendMessageUseCase = new SendMessageUseCase(_connectionManager);

        await sendMessageUseCase.Execute(message);
    }
}
