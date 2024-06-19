using AutoMapper;
using OrangeBranchTaskManager.Communication.DTOs;
using OrangeBranchTaskManager.Domain.Entities;
using OrangeBranchTaskManager.Infrastructure.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace OrangeBranchTaskManager.Application.UseCases.Task;

public class TaskUseCases : ITaskUseCases
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public TaskUseCases(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<TaskDTO> GetById(int id)
    {
        if (id <= 0) throw new ArgumentNullException("id");

        var task = await _unitOfWork.TaskRepository.GetByIdAsync(id);

        if (task is null)
            throw new KeyNotFoundException("Tarefa não encontrada");

        // Mapeia de TaskModel para TaskDTO
        TaskDTO result = _mapper.Map<TaskDTO>(task);

        return result;
    }

    public async Task<IEnumerable<TaskDTO>> GetTasks()
    {
        var tasks = await _unitOfWork.TaskRepository.GetAllAsync();

        if (!tasks.Any())
            throw new Exception("Nenhuma tarefa encontrada");

        IEnumerable<TaskDTO> result = _mapper.Map<IEnumerable<TaskDTO>>(tasks);

        return result;
    }

    public async Task<TaskDTO> CreateTask(TaskDTO taskData)
    {
        if (taskData is null) throw new ArgumentNullException(nameof(taskData));
        var task = _mapper.Map<TaskModel>(taskData);

        var addedTask = _unitOfWork.TaskRepository.CreateAsync(task);
        if (addedTask is null) throw new DbUpdateException("Erro ao criar tarefa");
        await _unitOfWork.CommitAsync();

        var result = _mapper.Map<TaskDTO>(addedTask);

        return result;
    }

    public async Task<TaskDTO> UpdateTask(int id, TaskDTO taskData)
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

    public async Task<TaskDTO> DeleteTask(int id)
    {
        if (id <= 0) throw new ArgumentNullException(nameof(id));

        var existingTask = await _unitOfWork.TaskRepository.GetByIdAsync(id);
        if (existingTask is null) throw new KeyNotFoundException();

        _unitOfWork.TaskRepository.DeleteAsync(existingTask);
        await _unitOfWork.CommitAsync();

        return _mapper.Map<TaskDTO>(existingTask);
    }
}
