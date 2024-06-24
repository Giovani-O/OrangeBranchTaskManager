﻿using AutoMapper;
using OrangeBranchTaskManager.Communication.DTOs;
using OrangeBranchTaskManager.Domain.Entities;
using OrangeBranchTaskManager.Exception;
using OrangeBranchTaskManager.Infrastructure.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using OrangeBranchTaskManager.Exception.ExceptionsBase;
using OrangeBranchTaskManager.Application.UseCases.SendMessage;
using OrangeBranchTaskManager.Infrastructure.RabbitMQConnectionManager;

namespace OrangeBranchTaskManager.Application.UseCases.Tasks.Create;

public class CreateTaskUseCase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IRabbitMQConnectionManager _connectionManager;

    public CreateTaskUseCase(
        IUnitOfWork unitOfWork, 
        IMapper mapper, 
        IRabbitMQConnectionManager connectionManager
    )
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _connectionManager = connectionManager;
    }

    public async Task<TaskDTO> Execute(TaskDTO taskData)
    {
        Validate(taskData);

        var task = _mapper.Map<TaskModel>(taskData);
        var addedTask = _unitOfWork.TaskRepository.CreateAsync(task);

        if (addedTask is null) throw new ErrorOnExecutionException(
            new Dictionary<string, List<string>>()
            {
                { "Error", new List<string>() { ResourceErrorMessages.ERROR_CREATE_TASK } }
            }
        );

        await _unitOfWork.CommitAsync();
        var result = _mapper.Map<TaskDTO>(addedTask);

        await SendMessage();

        return result;
    }

    private void Validate(TaskDTO request)
    {
        var validator = new CreateTaskValidator();
        var result = validator.Validate(request);

        if (!result.IsValid)
        {
            //var errorMessages = result.Errors.Select(x => x.ErrorMessage).ToList();
            //throw new ErrorOnValidationException(errorMessages);
            var errorDictionary = result.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(x => x.Key, x => x.Select(e => e.ErrorMessage).ToList());

            throw new ErrorOnValidationException(errorDictionary);
        }
    }

    private async Task SendMessage()
    {
        var message = new SendMessageDTO
        {
            Message = "Tarefa criada"
        };

        var sendMessageUseCase = new SendMessageUseCase(_connectionManager);

        await sendMessageUseCase.Execute(message);
    }
}