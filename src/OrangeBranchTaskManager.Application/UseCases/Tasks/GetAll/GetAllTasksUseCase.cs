using AutoMapper;
using OrangeBranchTaskManager.Communication.DTOs;
using OrangeBranchTaskManager.Domain.UnitOfWork;
using OrangeBranchTaskManager.Exception;
using OrangeBranchTaskManager.Exception.ExceptionsBase;

namespace OrangeBranchTaskManager.Application.UseCases.Tasks.GetAll;

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
            throw new ErrorOnExecutionException(
                new Dictionary<string, List<string>>()
                {
                    { "Error", new List<string>() { ResourceErrorMessages.ERROR_NOT_FOUND_TASKS } }
                }
            );

        IEnumerable<TaskDTO> result = _mapper.Map<IEnumerable<TaskDTO>>(tasks);

        return result;
    }
}
