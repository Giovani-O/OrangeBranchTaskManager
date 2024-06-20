using FluentValidation;
using Producer.Communication.DTOs;
using Producer.Exceptions;

namespace Producer.Application.UseCases.SendMessage;

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
