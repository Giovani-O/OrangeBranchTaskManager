using RabbitMQ.Client;

namespace OrangeBranchTaskManager.Infrastructure.RabbitMQConnectionManager;
public interface IRabbitMQConnectionManager
{
    IModel GetChannel();
}
