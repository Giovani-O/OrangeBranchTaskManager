using RabbitMQ.Client;

namespace Emailing.Domain.RabbitMQConnectionManager;
public interface IRabbitMQConnectionManager
{
    IModel GetChannel();
}
