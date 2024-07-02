using AutoMapper;
using OrangeBranchTaskManager.Application.UseCases.CurrentUser;
using OrangeBranchTaskManager.Application.UseCases.PublishMessage;
using OrangeBranchTaskManager.Application.UseCases.SendEmail;
using OrangeBranchTaskManager.Communication.DTOs;
using OrangeBranchTaskManager.Communication.Templates;
using OrangeBranchTaskManager.Domain.Entities;
using OrangeBranchTaskManager.Domain.RabbitMQConnectionManager;
using OrangeBranchTaskManager.Domain.UnitOfWork;
using OrangeBranchTaskManager.Exception;
using OrangeBranchTaskManager.Exception.ExceptionsBase;
using System.Text.Json;

namespace OrangeBranchTaskManager.Application.UseCases.Tasks.Create;

public class CreateTaskUseCase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ISendEmailUseCase _sendEmailUseCase;

    public CreateTaskUseCase(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ISendEmailUseCase sendEmailUseCase
    )
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _sendEmailUseCase = sendEmailUseCase;
    }

    public async Task<TaskDTO> Execute(TaskDTO taskData)
    {
        Validate(taskData);

        var task = _mapper.Map<TaskModel>(taskData);
        var addedTask = _unitOfWork.TaskRepository.CreateAsync(task);

        if (addedTask is null) throw new ErrorOnExecutionException(
            new Dictionary<string, List<string>>()
            {
                { "Error", new List<string>() { ResourceErrorMessages.ERROR_CREATE_TASK } }
            }
        );

        await _unitOfWork.CommitAsync();
        var result = _mapper.Map<TaskDTO>(addedTask);

        await _sendEmailUseCase.Execute(taskData);

        return result;
    }

    private void Validate(TaskDTO request)
    {
        var validator = new CreateTaskValidator();
        var result = validator.Validate(request);

        if (!result.IsValid)
        {
            var errorDictionary = result.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(x => x.Key, x => x.Select(e => e.ErrorMessage).ToList());

            throw new ErrorOnValidationException(errorDictionary);
        }
    }
}
