using FluentValidation;
using OrangeBranchTaskManager.Communication.DTOs;
using OrangeBranchTaskManager.Exception;

namespace OrangeBranchTaskManager.Application.UseCases.SendMessage;

public class SendMessageValidator : AbstractValidator<SendMessageDTO>
{
    public SendMessageValidator()
    {
        RuleFor(request => request.Message)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(ResourceErrorMessages.ERROR_EMPTY_MESSAGE)
            .WithName(nameof(SendMessageDTO.Message));
    }
}
