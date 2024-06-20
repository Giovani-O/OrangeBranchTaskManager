using FluentValidation;
using OrangeBranchTaskManager.Communication.DTOs;
using OrangeBranchTaskManager.Exception;

namespace OrangeBranchTaskManager.Application.UseCases.Authentication.Login;
public class LoginValidator : AbstractValidator<LoginDTO>
{
    public LoginValidator()
    {
        RuleFor(task => task.Email).NotEmpty().WithMessage(ResourceErrorMessages.ERROR_EMAIL_EMPTY);
        RuleFor(task => task.Email).EmailAddress().WithMessage(ResourceErrorMessages.ERROR_INVALID_EMAIL);
        RuleFor(task => task.Password).NotEmpty().WithMessage(ResourceErrorMessages.ERROR_PASSWORD_EMPTY);
    }
}
