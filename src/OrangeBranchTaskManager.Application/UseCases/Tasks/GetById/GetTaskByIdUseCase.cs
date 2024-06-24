using AutoMapper;
using OrangeBranchTaskManager.Communication.DTOs;
using OrangeBranchTaskManager.Domain.UnitOfWork;
using OrangeBranchTaskManager.Exception;
using OrangeBranchTaskManager.Exception.ExceptionsBase;

namespace OrangeBranchTaskManager.Application.UseCases.Tasks.GetById;

public class GetTaskByIdUseCase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetTaskByIdUseCase(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<TaskDTO> Execute(int id)
    {
        Validate(id);

        var task = await _unitOfWork.TaskRepository.GetByIdAsync(id);

        if (task is null)
            throw new ErrorOnExecutionException(
                new Dictionary<string, List<string>>()
                {
                    { "Error", new List<string>() { ResourceErrorMessages.ERROR_NOT_FOUND_TASK } }
                }
            );

        TaskDTO result = _mapper.Map<TaskDTO>(task);

        return result;
    }

    private void Validate(int id)
    {
        var validator = new GetTaskByIdValidator();
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
