using RabbitMQ.Client;

namespace OrangeBranchTaskManager.Domain.RabbitMQConnectionManager;
public interface IRabbitMQConnectionManager
{
    IModel GetChannel();
}
