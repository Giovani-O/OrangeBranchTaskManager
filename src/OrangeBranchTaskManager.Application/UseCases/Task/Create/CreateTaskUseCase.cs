using AutoMapper;
using OrangeBranchTaskManager.Communication.DTOs;
using OrangeBranchTaskManager.Domain.Entities;
using OrangeBranchTaskManager.Exception;
using OrangeBranchTaskManager.Infrastructure.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace OrangeBranchTaskManager.Application.UseCases.Task.Create;

public class CreateTaskUseCase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateTaskUseCase(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<TaskDTO> Execute(TaskDTO taskData)
    {
        if (taskData is null) throw new ArgumentNullException(nameof(taskData));
        var task = _mapper.Map<TaskModel>(taskData);

        var addedTask = _unitOfWork.TaskRepository.CreateAsync(task);
        if (addedTask is null) throw new DbUpdateException(ResourceErrorMessages.ERROR_CREATE_TASK);
        await _unitOfWork.CommitAsync();

        var result = _mapper.Map<TaskDTO>(addedTask);

        return result;
    }
}
