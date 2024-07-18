using AutoMapper;
using OrangeBranchTaskManager.Application.UseCases.CurrentUser;
using OrangeBranchTaskManager.Application.UseCases.PublishMessage;
using OrangeBranchTaskManager.Communication.DTOs;
using OrangeBranchTaskManager.Communication.Templates;
using OrangeBranchTaskManager.Domain.Entities;
using OrangeBranchTaskManager.Domain.RabbitMQConnectionManager;
using OrangeBranchTaskManager.Domain.UnitOfWork;
using OrangeBranchTaskManager.Exception;
using OrangeBranchTaskManager.Exception.ExceptionsBase;
using System.Text.Json;
using OrangeBranchTaskManager.Application.UseCases.SendEmail;

namespace OrangeBranchTaskManager.Application.UseCases.Tasks.Update;

public class UpdateTaskUseCase : IUpdateTaskUseCase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ISendEmailUseCase _sendEmailUseCase;

    public UpdateTaskUseCase(
        IUnitOfWork unitOfWork, 
        IMapper mapper, 
        ISendEmailUseCase sendEmailUseCase
    )
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _sendEmailUseCase = sendEmailUseCase;
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
                { ResourceErrorMessages.ERROR, new List<string>() { ResourceErrorMessages.ERROR_NOT_FOUND_TASK } }
            }
        );

        _mapper.Map(taskData, existingTask);

        _unitOfWork.TaskRepository.UpdateAsync(existingTask);
        await _unitOfWork.CommitAsync();

        var result = _mapper.Map<TaskDTO>(existingTask);

        await _sendEmailUseCase.UpdateTaskExecute(taskData);

        return result;
    }

    private void Validate(TaskDTO request)
    {
        var validator = new UpdateTaskValidator();
        var result = validator.Validate(request);

        if (result.IsValid) return;
        
        var errorDictionary = result.Errors
            .GroupBy(e => e.PropertyName)
            .ToDictionary(x => x.Key, x => x.Select(e => e.ErrorMessage).ToList());

        throw new ErrorOnValidationException(errorDictionary);
    }
}
