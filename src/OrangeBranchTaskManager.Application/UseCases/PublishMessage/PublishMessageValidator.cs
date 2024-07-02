using FluentValidation;
using OrangeBranchTaskManager.Communication.DTOs;
using OrangeBranchTaskManager.Exception;

namespace OrangeBranchTaskManager.Application.UseCases.PublishMessage;

public class PublishMessageValidator : AbstractValidator<PublishMessageDTO>
{
    public PublishMessageValidator()
    {
        RuleFor(request => request.Message)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(ResourceErrorMessages.ERROR_EMPTY_MESSAGE)
            .WithName(nameof(PublishMessageDTO.Message));
    }
}
