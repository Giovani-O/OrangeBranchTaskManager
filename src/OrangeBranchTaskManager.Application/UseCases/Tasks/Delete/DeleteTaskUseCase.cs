using AutoMapper;
using OrangeBranchTaskManager.Communication.DTOs;
using OrangeBranchTaskManager.Exception;
using OrangeBranchTaskManager.Exception.ExceptionsBase;
using OrangeBranchTaskManager.Infrastructure.UnitOfWork;

namespace OrangeBranchTaskManager.Application.UseCases.Tasks.Delete;

public class DeleteTaskUseCase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public DeleteTaskUseCase(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
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
}
