using Emailing.Application.UseCases.ConsumeMessage;
using Emailing.Application.UseCases.SendEmail;
using Emailing.Domain.RabbitMQConnectionManager;
using RabbitMQ.Client;

namespace Emailing.Worker;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IRabbitMQConnectionManager _connectionManager;
    private readonly ISendEmailUseCase _sendEmail;

    public Worker(ILogger<Worker> logger, IRabbitMQConnectionManager connectionManager, ISendEmailUseCase sendEmail)
    {
        _logger = logger;
        _connectionManager = connectionManager;
        _sendEmail = sendEmail;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var useCase = new ConsumeMessageUseCase(_connectionManager, _sendEmail);

        while (!stoppingToken.IsCancellationRequested)
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation("[*] Aguardando mensagens...");
            }
            useCase.Execute();

            await Task.Delay(5000, stoppingToken);
        }
    }
}
