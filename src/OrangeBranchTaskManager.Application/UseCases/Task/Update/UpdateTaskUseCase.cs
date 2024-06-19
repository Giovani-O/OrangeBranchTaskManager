using AutoMapper;
using OrangeBranchTaskManager.Communication.DTOs;
using OrangeBranchTaskManager.Infrastructure.UnitOfWork;

namespace OrangeBranchTaskManager.Application.UseCases.Task.Update;

public class UpdateTaskUseCase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateTaskUseCase(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<TaskDTO> Execute(int id, TaskDTO taskData)
    {
        if (taskData is null) throw new ArgumentNullException(nameof(taskData));
        if (id <= 0 || id != taskData.Id) throw new ArgumentNullException("id");

        // Busca task no banco de dados
        var existingTask = await _unitOfWork.TaskRepository.GetByIdAsync(id);
        if (existingTask is null) throw new KeyNotFoundException();

        // Mapeia dados atualizados na task existente
        _mapper.Map(taskData, existingTask);

        // Modifica task e confirma alteração no banco
        _unitOfWork.TaskRepository.UpdateAsync(existingTask);
        await _unitOfWork.CommitAsync();

        var result = _mapper.Map<TaskDTO>(existingTask);

        return result;
    }
}
