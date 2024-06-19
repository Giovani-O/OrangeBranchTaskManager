using AutoMapper;
using OrangeBranchTaskManager.Application.UseCases.Task.Delete;
using OrangeBranchTaskManager.Communication.DTOs;
using OrangeBranchTaskManager.Exception;
using OrangeBranchTaskManager.Exception.ExceptionsBase;
using OrangeBranchTaskManager.Infrastructure.UnitOfWork;

namespace OrangeBranchTaskManager.Application.UseCases.Task.GetById;

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
            throw new OrangeBranchTaskManagerException(ResourceErrorMessages.ERROR_NOT_FOUND_TASK);

        // Mapeia de TaskModel para TaskDTO
        TaskDTO result = _mapper.Map<TaskDTO>(task);

        return result;
    }

    private void Validate(int id)
    {
        var validator = new GetTaskByIdValidator();
        var result = validator.Validate(id);

        if (!result.IsValid)
        {
            var errorMessages = result.Errors.Select(x => x.ErrorMessage).ToList();
            throw new ErrorOnValidationException(errorMessages);
        }
    }
}
