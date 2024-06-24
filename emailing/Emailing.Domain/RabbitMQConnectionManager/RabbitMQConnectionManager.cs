using RabbitMQ.Client;

namespace Emailing.Domain.RabbitMQConnectionManager;

public class RabbitMQConnectionManager : IRabbitMQConnectionManager
{
    private readonly ConnectionFactory _factory;
    private readonly IConnection _connection;
    private readonly IModel _channel;

    public RabbitMQConnectionManager()
    {
        _factory = new ConnectionFactory { HostName = "localhost" };
        _connection = _factory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.QueueDeclare(
            queue: "OrangeBranchTaskManager.Emails",
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);
    }

    public static RabbitMQConnectionManager Instance { get; } = new RabbitMQConnectionManager();

    public IModel Channel { get => _channel; }

    public IModel GetChannel()
    {
        throw new NotImplementedException();
    }
}
