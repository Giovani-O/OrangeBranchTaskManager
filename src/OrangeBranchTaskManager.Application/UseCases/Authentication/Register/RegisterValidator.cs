using FluentValidation;
using OrangeBranchTaskManager.Communication.DTOs;
using OrangeBranchTaskManager.Exception;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrangeBranchTaskManager.Application.UseCases.Authentication.Register;
public class RegisterValidator : AbstractValidator<RegisterDTO>
{
    public RegisterValidator()
    {
        RuleFor(task => task.Username).NotEmpty().WithMessage(ResourceErrorMessages.ERROR_USERNAME_EMPTY);
        RuleFor(task => task.Username).MaximumLength(100).WithMessage(ResourceErrorMessages.ERROR_USERNAME_TOO_LONG);
        RuleFor(task => task.Email).NotEmpty().WithMessage(ResourceErrorMessages.ERROR_EMAIL_EMPTY);
        RuleFor(task => task.Email).EmailAddress().WithMessage(ResourceErrorMessages.ERROR_INVALID_EMAIL);
        RuleFor(task => task.Email).MaximumLength(100).WithMessage(ResourceErrorMessages.ERROR_EMAIL_TOO_LONG);
        RuleFor(task => task.Password).NotEmpty().WithMessage(ResourceErrorMessages.ERROR_PASSWORD_EMPTY);
        RuleFor(task => task.Password).MaximumLength(100).WithMessage(ResourceErrorMessages.ERROR_PASSWORD_TOO_LONG);
    }
}