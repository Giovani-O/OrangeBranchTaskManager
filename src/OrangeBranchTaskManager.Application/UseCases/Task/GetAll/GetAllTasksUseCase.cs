using AutoMapper;
using OrangeBranchTaskManager.Communication.DTOs;
using OrangeBranchTaskManager.Exception;
using OrangeBranchTaskManager.Infrastructure.UnitOfWork;
using OrangeBranchTaskManager.Exception.ExceptionsBase;

namespace OrangeBranchTaskManager.Application.UseCases.Task.GetAll;

public class GetAllTasksUseCase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllTasksUseCase(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<TaskDTO>> Execute()
    {
        var tasks = await _unitOfWork.TaskRepository.GetAllAsync();

        if (!tasks.Any())
            throw new InvalidOperationException(ResourceErrorMessages.ERROR_NOT_FOUND_TASKS);

        IEnumerable<TaskDTO> result = _mapper.Map<IEnumerable<TaskDTO>>(tasks);

        return result;
    }
}
