using RabbitMQ.Client;

namespace Producer.Infrastructure.ConnectionManager;
public interface IRabbitMQConnectionManager
{
    IModel GetChannel();
}
