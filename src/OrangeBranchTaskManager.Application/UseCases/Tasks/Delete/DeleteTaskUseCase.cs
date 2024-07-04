using AutoMapper;
using OrangeBranchTaskManager.Application.UseCases.CurrentUser;
using OrangeBranchTaskManager.Application.UseCases.PublishMessage;
using OrangeBranchTaskManager.Communication.DTOs;
using OrangeBranchTaskManager.Communication.Templates;
using OrangeBranchTaskManager.Domain.RabbitMQConnectionManager;
using OrangeBranchTaskManager.Domain.UnitOfWork;
using OrangeBranchTaskManager.Exception;
using OrangeBranchTaskManager.Exception.ExceptionsBase;
using System.Text.Json;
using System.Threading.Tasks;
using OrangeBranchTaskManager.Application.UseCases.SendEmail;

namespace OrangeBranchTaskManager.Application.UseCases.Tasks.Delete;

public class DeleteTaskUseCase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ISendEmailUseCase _sendEmailUseCase;

    public DeleteTaskUseCase(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ISendEmailUseCase sendEmailUseCase
    )
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _sendEmailUseCase = sendEmailUseCase;
    }

    public async Task<TaskDTO> Execute(int id)
    {
        Validate(id);

        var existingTask = await _unitOfWork.TaskRepository.GetByIdAsync(id);
        if (existingTask is null) throw new ErrorOnExecutionException(
            new Dictionary<string, List<string>>()
            {
                { ResourceErrorMessages.ERROR, new List<string>() { ResourceErrorMessages.ERROR_DELETE_TASK } }
            }
        );

        _unitOfWork.TaskRepository.DeleteAsync(existingTask);
        await _unitOfWork.CommitAsync();

        await _sendEmailUseCase.DeleteTaskExecute(existingTask.Title!);

        return _mapper.Map<TaskDTO>(existingTask);
    }

    private void Validate(int id)
    {
        var validator = new DeleteTaskValidator();
        var result = validator.Validate(id);

        if (result.IsValid) return;
        
        var errorDictionary = result.Errors
            .GroupBy(e => e.PropertyName)
            .ToDictionary(x => x.Key, x => x.Select(e => e.ErrorMessage).ToList());

        throw new ErrorOnValidationException(errorDictionary);
    }
}
