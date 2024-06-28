using Emailing.Application.UseCases.SendEmail;
using Emailing.Communication.Templates;
using Emailing.Domain.Enums;
using Emailing.Domain.RabbitMQConnectionManager;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace Emailing.Application.UseCases.ConsumeMessage;

public class ConsumeMessageUseCase : IConsumeMessageUseCase
{
    private readonly IRabbitMQConnectionManager _connectionManager;
    private readonly ISendEmailUseCase _sendEmail;

    public ConsumeMessageUseCase(IRabbitMQConnectionManager connectionManager, ISendEmailUseCase sendEmail)
    {
        _connectionManager = connectionManager;
        _sendEmail = sendEmail;
    }

    public void Execute()
    {
        var channel = _connectionManager.GetChannel();
        var consumer = new EventingBasicConsumer(channel);

        consumer.Received += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            await _sendEmail.Execute(message);
        };

        channel.BasicConsume(
            queue: "OrangeBranchTaskManager.Emails",
            autoAck: true,
            consumer: consumer
        );
    }
    
}
