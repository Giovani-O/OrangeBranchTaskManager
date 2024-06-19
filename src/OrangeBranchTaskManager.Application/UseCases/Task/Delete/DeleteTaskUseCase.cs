using AutoMapper;
using OrangeBranchTaskManager.Communication.DTOs;
using OrangeBranchTaskManager.Infrastructure.UnitOfWork;

namespace OrangeBranchTaskManager.Application.UseCases.Task.Delete;

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
        if (id <= 0) throw new ArgumentNullException(nameof(id));

        var existingTask = await _unitOfWork.TaskRepository.GetByIdAsync(id);
        if (existingTask is null) throw new KeyNotFoundException();

        _unitOfWork.TaskRepository.DeleteAsync(existingTask);
        await _unitOfWork.CommitAsync();

        return _mapper.Map<TaskDTO>(existingTask);
    }
}
