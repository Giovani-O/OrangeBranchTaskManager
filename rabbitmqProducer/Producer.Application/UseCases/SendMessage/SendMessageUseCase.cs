﻿using Producer.Infrastructure.ConnectionManager;
using Producer.Communication.DTOs;
using Producer.Exceptions.ExceptionBase;
using System.Text;

namespace Producer.Application.UseCases.SendMessage;

public class SendMessageUseCase
{
    private readonly IRabbitMQConnectionManager _connectionManager;

    public SendMessageUseCase(IRabbitMQConnectionManager connectionManager)
    {
        _connectionManager = connectionManager;
    }

    public async Task Execute(SendMessageDTO request)
    {
        Validate(request);

        var channel = _connectionManager.GetChannel();
        await Task.Run(() => channel.BasicPublish(
            exchange: string.Empty,
            routingKey: "OrangeBranchTaskManager.Emails",
            mandatory: false,
            basicProperties: null,
            body: Encoding.UTF8.GetBytes(request.Message))
        );
    }

    private void Validate(SendMessageDTO request)
    {
        var validator = new SendMessageValidator();
        var result = validator.Validate(request);

        if (!result.IsValid) 
        {
            var errorDictionary = result.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(x => x.Key, x => x.Select(e => e.ErrorMessage).ToList());

            throw new ErrorOnSendException(errorDictionary);
        }
    }
}
 