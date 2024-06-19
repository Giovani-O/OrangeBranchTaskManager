using AutoMapper;
using OrangeBranchTaskManager.Communication.DTOs;
using OrangeBranchTaskManager.Exception;
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
        if (id <= 0) throw new ArgumentNullException("id");

        var task = await _unitOfWork.TaskRepository.GetByIdAsync(id);

        if (task is null)
            throw new KeyNotFoundException(ResourceErrorMessages.ERROR_NOT_FOUND_TASK);

        // Mapeia de TaskModel para TaskDTO
        TaskDTO result = _mapper.Map<TaskDTO>(task);

        return result;
    }
}
