using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrangeBranchTaskManager.Api.Data;
using OrangeBranchTaskManager.Api.DTOs;
using OrangeBranchTaskManager.Api.Models;

namespace OrangeBranchTaskManager.Api.Services;

public class TasksService : ITasksService
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public TasksService(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<TaskDTO> GetById(int id)
    {
        if (id <= 0) throw new ArgumentNullException("id");

        var task = await _context.Tasks.FirstOrDefaultAsync(x => x.Id == id);

        if (task is null)
            throw new Exception("Erro ao buscar tarefa");

        // Mapeia de TaskModel para TaskDTO
        TaskDTO result = _mapper.Map<TaskDTO>(task);

        return result;
    }

    public async Task<IEnumerable<TaskDTO>> GetTasks()
    {
        var tasks = await _context.Tasks.ToListAsync();

        if (tasks is null)
            throw new Exception("Erro ao buscar lista de tarefas");

        IEnumerable<TaskDTO> result = _mapper.Map<IEnumerable<TaskDTO>>(tasks);

        return result;
    }

    public async Task<TaskDTO> CreateTask(TaskDTO taskData)
    {
        if (taskData is null) throw new ArgumentNullException(nameof(taskData));
        var task = _mapper.Map<TaskModel>(taskData);

        var addedTask = _context.Tasks.Add(task);
        await _context.SaveChangesAsync();

        if (addedTask is null) throw new Exception("Erro ao criar tarefa");

        var result = _mapper.Map<TaskDTO>(addedTask.Entity);

        return result;
    }

    public async Task<TaskDTO> UpdateTask(int id, TaskDTO taskData)
    {
        if (taskData is null) throw new ArgumentNullException(nameof(taskData));
        if (id <= 0 || id != taskData.Id) throw new ArgumentNullException("id");

        // Busca task no banco de dados
        var existingTask = await _context.Tasks.FindAsync(id);
        if (existingTask is null) throw new KeyNotFoundException();

        // Mapeia dados atualizados na task existente
        _mapper.Map(taskData, existingTask);

        // Modifica task e confirma alteração no banco
        _context.Entry(existingTask).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        var result = _mapper.Map<TaskDTO>(existingTask);

        return result;
    }

    public async Task<TaskDTO> DeleteTask(int id)
    {
        if (id <= 0) throw new ArgumentNullException(nameof(id));

        var existingTask = await _context.Tasks.FindAsync(id);
        if (existingTask is null) throw new KeyNotFoundException();

        _context.Tasks.Remove(existingTask);
        await _context.SaveChangesAsync();

        return _mapper.Map<TaskDTO>(existingTask);
    }
}
