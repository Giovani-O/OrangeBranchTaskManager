﻿using AutoMapper;
using OrangeBranchTaskManager.Application.UseCases.Task.Create;
using OrangeBranchTaskManager.Communication.DTOs;
using OrangeBranchTaskManager.Exception;
using OrangeBranchTaskManager.Exception.ExceptionsBase;
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
        Validate(taskData);

        if (id <= 0 || id != taskData.Id) throw new OrangeBranchTaskManagerException(ResourceErrorMessages.UNKNOWN_ERROR);

        // Busca task no banco de dados
        var existingTask = await _unitOfWork.TaskRepository.GetByIdAsync(id);
        if (existingTask is null) throw new OrangeBranchTaskManagerException(ResourceErrorMessages.ERROR_NOT_FOUND_TASK);

        // Mapeia dados atualizados na task existente
        _mapper.Map(taskData, existingTask);

        // Modifica task e confirma alteração no banco
        _unitOfWork.TaskRepository.UpdateAsync(existingTask);
        await _unitOfWork.CommitAsync();

        var result = _mapper.Map<TaskDTO>(existingTask);

        return result;
    }

    private void Validate(TaskDTO request)
    {
        var validator = new UpdateTaskValidator();
        var result = validator.Validate(request);

        if (!result.IsValid)
        {
            var errorMessages = result.Errors.Select(x => x.ErrorMessage).ToList();
            throw new ErrorOnValidationException(errorMessages);
        }
    }
}