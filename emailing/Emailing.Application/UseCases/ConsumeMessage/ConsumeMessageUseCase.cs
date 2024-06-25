using Emailing.Domain.RabbitMQConnectionManager;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Emailing.Application.UseCases.ConsumeMessage;

public class ConsumeMessageUseCase
{
    private readonly IRabbitMQConnectionManager _connectionManager;

    public ConsumeMessageUseCase(IRabbitMQConnectionManager connectionManager)
    {
        _connectionManager = connectionManager;
    }

    public void Execute()
    {
        var channel = _connectionManager.GetChannel();
        var consumer = new EventingBasicConsumer(channel);

        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine($"[!] New message: {message}");
        };

        channel.BasicConsume(
            queue: "OrangeBranchTaskManager.Emails",
            autoAck: true,
            consumer: consumer
        );
    } 
}
