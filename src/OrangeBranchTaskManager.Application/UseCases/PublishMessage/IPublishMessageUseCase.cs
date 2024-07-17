using OrangeBranchTaskManager.Communication.DTOs;

namespace OrangeBranchTaskManager.Application.UseCases.PublishMessage;

public interface IPublishMessageUseCase
{
    Task Execute(PublishMessageDTO request);
}