using OrangeBranchTaskManager.Domain.RabbitMQConnectionManager;
using OrangeBranchTaskManager.Communication.DTOs;
using OrangeBranchTaskManager.Exception.ExceptionsBase;
using System.Threading.Tasks;
using System.Text;

namespace OrangeBranchTaskManager.Application.UseCases.PublishMessage;

public class PublishMessageUseCase
{
    private readonly IRabbitMQConnectionManager _connectionManager;

    public PublishMessageUseCase(IRabbitMQConnectionManager connectionManager)
    {
        _connectionManager = connectionManager;
    }

    public async Task Execute(PublishMessageDTO request)
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

    private void Validate(PublishMessageDTO request)
    {
        var validator = new PublishMessageValidator();
        var result = validator.Validate(request);

        if (!result.IsValid) 
        {
            var errorDictionary = result.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(x => x.Key, x => x.Select(e => e.ErrorMessage).ToList());

            throw new ErrorOnSendMessageException(errorDictionary);
        }
    }
}
 