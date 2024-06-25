using Emailing.Application.UseCases.ConsumeMessage;
using Emailing.Domain.RabbitMQConnectionManager;
using RabbitMQ.Client;

namespace Emailing.Worker;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IRabbitMQConnectionManager _connectionManager;

    public Worker(ILogger<Worker> logger, IRabbitMQConnectionManager connectionManager)
    {
        _logger = logger;
        _connectionManager = connectionManager;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var useCase = new ConsumeMessageUseCase(_connectionManager);

        while (!stoppingToken.IsCancellationRequested)
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation("[*] Watching for messages...");
            }
            useCase.Execute();

            await Task.Delay(1000, stoppingToken);
        }
    }
}
